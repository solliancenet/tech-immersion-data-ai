using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Bulkhead;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TweetGenerator.OutputHelpers;

namespace TweetGenerator
{
    internal class Program
    {
        private static DocumentClient _cosmosDbClient;
        private static IConfigurationRoot _configuration;

        private const string DatabaseName = "ContosoAuto";
        private const string CollectionName = "tweets";
        private const string PartitionKey = "/user/location";

        private static readonly object LockObject = new object();
        // AutoResetEvent to signal when to exit the application.
        private static readonly AutoResetEvent WaitHandle = new AutoResetEvent(false);
        // Track Cosmos DB statistics.
        // At any time, requests pending = made - succeeded - failed.
        private static long _totalMessages = 0;
        private static long _cosmosRequestsMade = 0;
        private static long _cosmosRequestsSucceeded = 0;
        private static long _cosmosRequestsFailed = 0;
        private static long _cosmosRequestsSucceededInBatch = 0;
        private static long _cosmosElapsedTime = 0;
        private static long _cosmosTotalElapsedTime = 0;
        private static double _cosmosRUsPerBatch = 0;

        private static readonly Statistic[] LatestStatistics = new Statistic[0];

        // Place Cosmos DB calls into bulkhead to prevent thread starvation caused by failing or waiting calls.
        // Let any number (int.MaxValue) of calls _queue for an execution slot in the bulkhead to allow the generator to send as many calls as possible.
        private const int MaxParallelization = 60;
        private static readonly BulkheadPolicy BulkheadForCosmosDbCalls = Policy.BulkheadAsync(MaxParallelization, int.MaxValue);

        static void Main(string[] args)
        {
            // Setup configuration to either read from the appsettings.json file (if present) or environment variables.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var arguments = ParseArguments();
            var cosmosDbConnectionString = new CosmosDbConnectionString(arguments.CosmosDbConnectionString);
            // Set an optional timeout for the generator.
            var cancellationSource = arguments.MillisecondsToRun == 0 ? new CancellationTokenSource() : new CancellationTokenSource(arguments.MillisecondsToRun);
            var cancellationToken = cancellationSource.Token;
            var statistics = new Statistic[0];

            // Set the Cosmos DB connection policy.
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            var numberOfMillisecondsToLead = arguments.MillisecondsToLead;

            var taskWaitTime = 0;

            if (numberOfMillisecondsToLead > 0)
            {
                taskWaitTime = numberOfMillisecondsToLead;
            }

            var progress = new Progress<Progress>();
            progress.ProgressChanged += (sender, progressArgs) =>
            {
                foreach (var message in progressArgs.Messages)
                {
                    WriteLineInColor(message.Message, message.Color.ToConsoleColor());
                }
                statistics = progressArgs.Statistics;
            };

            WriteLineInColor("Tweet Generator", ConsoleColor.White);
            Console.WriteLine("======");
            WriteLineInColor("Press Ctrl+C or Ctrl+Break to cancel.", ConsoleColor.Cyan);
            Console.WriteLine("Statistics for generated tweet data will be updated for every 60 tweets sent.");
            Console.WriteLine(string.Empty);

            ThreadPool.SetMinThreads(100, 100);

            // Handle Control+C or Control+Break.
            Console.CancelKeyPress += (o, e) =>
            {
                WriteLineInColor("Stopped generator. No more tweets are being sent.", ConsoleColor.Yellow);
                cancellationSource.Cancel();

                // Allow the main thread to continue and exit...
                WaitHandle.Set();

                OutputStatistics(statistics);
            };

            // Initialize the telemetry generator:
            TweetGenerator.Init();

            // Instantiate Cosmos DB client and start sending messages:
            using (_cosmosDbClient = new DocumentClient(cosmosDbConnectionString.ServiceEndpoint, cosmosDbConnectionString.AuthKey, connectionPolicy))
            {
                InitializeCosmosDb().Wait();

                // Find and output the collection details, including # of RU/s.
                var dataCollection = GetCollectionIfExists(DatabaseName, CollectionName);
                var offer = (OfferV2)_cosmosDbClient.CreateOfferQuery().Where(o => o.ResourceLink == dataCollection.SelfLink).AsEnumerable().FirstOrDefault();
                if (offer != null)
                {
                    var currentCollectionThroughput = offer.Content.OfferThroughput;
                    WriteLineInColor($"Found collection `{CollectionName}` with {currentCollectionThroughput} RU/s ({currentCollectionThroughput} reads/second; {currentCollectionThroughput / 5} writes/second @ 1KB doc size)", ConsoleColor.Green);
                }

                // Start sending data to both Event Hubs and Cosmos DB.
                SendData(taskWaitTime, cancellationToken, progress).Wait();
            }

            cancellationSource.Cancel();
            Console.WriteLine();
            WriteLineInColor("Done sending generated vehicle telemetry data", ConsoleColor.Cyan);
            Console.WriteLine();
            Console.WriteLine();

            OutputStatistics(statistics);

            // Keep the console open.
            Console.ReadLine();
            WaitHandle.WaitOne();
        }

        #region Send Data

        private static async Task SendData(int waitTime, CancellationToken externalCancellationToken, IProgress<Progress> progress)
        {
            if (waitTime > 0)
            {
                var span = TimeSpan.FromMilliseconds(waitTime);
                await Task.Delay(span, externalCancellationToken);
            }

            if (externalCancellationToken == null) throw new ArgumentNullException(nameof(externalCancellationToken));
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            // Perform garbage collection prior to timing for statistics.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var internalCancellationTokenSource = new CancellationTokenSource();
            var combinedToken = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken, internalCancellationTokenSource.Token).Token;

            var tasks = new List<Task>();
            var messages = new ConcurrentQueue<ColoredMessage>();

            var cosmosTimer = new Stopwatch();

            // Create the Cosmos DB collection URI:
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);

            // Ensure none of what follows runs synchronously.
            await Task.FromResult(true).ConfigureAwait(false);

            // Continue while cancellation is not requested.
            while (!combinedToken.IsCancellationRequested)
            {
                if (externalCancellationToken.IsCancellationRequested)
                {
                    return;
                }

                for (int i = 0; i <= 4; i++)
                {
                    _totalMessages++;
                    var thisRequest = _totalMessages;

                    var tweet = TweetGenerator.GenerateTweet();

                    #region Write to Cosmos DB

                    _cosmosRequestsMade++;
                    tasks.Add(BulkheadForCosmosDbCalls.ExecuteAsync(async ct =>
                    {
                        try
                        {
                            cosmosTimer.Start();

                            // Send to Cosmos DB:
                            var response = await _cosmosDbClient.CreateDocumentAsync(collectionUri, tweet)
                                .ConfigureAwait(false);
                            //Console.WriteLine(tweet.Text);

                            cosmosTimer.Stop();
                            _cosmosElapsedTime = cosmosTimer.ElapsedMilliseconds;

                            // Keep running total of RUs consumed:
                            _cosmosRUsPerBatch += response.RequestCharge;

                            _cosmosRequestsSucceededInBatch++;
                        }
                        catch (DocumentClientException de)
                        {
                            if (!ct.IsCancellationRequested) messages.Enqueue(new ColoredMessage($"Cosmos DB request {thisRequest} eventually failed with: {de.Message}; Retry-after: {de.RetryAfter.TotalSeconds} seconds.", Color.Red));

                            _cosmosRequestsFailed++;
                        }
                        catch (Exception e)
                        {
                            if (!ct.IsCancellationRequested) messages.Enqueue(new ColoredMessage($"Cosmos DB request {thisRequest} eventually failed with: {e.Message}", Color.Red));

                            _cosmosRequestsFailed++;
                        }
                    }, combinedToken)
                        .ContinueWith((t, k) =>
                        {
                            if (t.IsFaulted) messages.Enqueue(new ColoredMessage($"Request to Cosmos DB failed with: {t.Exception?.Flatten().InnerExceptions.First().Message}", Color.Red));

                            _cosmosRequestsFailed++;
                        }, thisRequest, TaskContinuationOptions.NotOnRanToCompletion)
                    );

                    #endregion Write to Cosmos DB

                    if (i == 4)
                    {
                        var span = TimeSpan.FromMilliseconds(2000);
                        await Task.Delay(span, externalCancellationToken);
                    }
                }

                if (_totalMessages % 60 == 0)
                {
                    cosmosTimer.Stop();
                    _cosmosTotalElapsedTime += _cosmosElapsedTime;
                    _cosmosRequestsSucceeded += _cosmosRequestsSucceededInBatch;

                    // Calculate RUs/second/month:
                    var ruPerSecond = (_cosmosRUsPerBatch / (_cosmosElapsedTime * .001));
                    var ruPerMonth = ruPerSecond * 86400 * 30;

                    // Random delay every 1000 messages that are sent.
                    //await Task.Delay(random.Next(100, 1000), externalCancellationToken).ConfigureAwait(false);

                    // The obvious and recommended method for sending a lot of data is to do so in batches. This method can
                    // multiply the amount of data sent with each request by hundreds or thousands. However, the point of
                    // our exercise is not to maximize throughput and send as much data as possible, but to compare the
                    // relative performance between Event Hubs and Cosmos DB.

                    // Output statistics. Be on the lookout for the following:
                    //  - Inserted line shows successful inserts in this batch and throughput for writes/second with RU/s usage and estimated monthly ingestion rate added to Cosmos DB statistics.
                    //  - Processing time: Processing time for the past 1,000 requested inserts.
                    //  - Total elapsed time: Running total of time taken to process all documents.
                    //  - Succeeded shows number of accumulative successful inserts to the service.
                    //  - Pending are items in the bulkhead queue. This amount will continue to grow if the service is unable to keep up with demand.
                    //  - Accumulative failed requests that encountered an exception.
                    messages.Enqueue(new ColoredMessage($"Total requests: requested {_totalMessages:00} ", Color.Cyan));
                    messages.Enqueue(new ColoredMessage(string.Empty));
                    messages.Enqueue(new ColoredMessage($"Inserted {_cosmosRequestsSucceededInBatch:00} docs @ {(_cosmosRequestsSucceededInBatch / (_cosmosElapsedTime * .001)):0.00} writes/s, {ruPerSecond:0.00} RU/s ({(ruPerMonth / (1000 * 1000 * 1000)):0.00}B max monthly 1KB writes) ", Color.White));
                    messages.Enqueue(new ColoredMessage($"Processing time {_cosmosElapsedTime} ms", Color.Magenta));
                    messages.Enqueue(new ColoredMessage($"Total elapsed time {(_cosmosTotalElapsedTime * .001):0.00} seconds", Color.Magenta));
                    messages.Enqueue(new ColoredMessage($"Total succeeded {_cosmosRequestsSucceeded:00} ", Color.Green));
                    messages.Enqueue(new ColoredMessage($"Total pending {_cosmosRequestsMade - _cosmosRequestsSucceeded - _cosmosRequestsFailed:00} ", Color.Yellow));
                    messages.Enqueue(new ColoredMessage($"Total failed {_cosmosRequestsFailed:00}", Color.Red));
                    messages.Enqueue(new ColoredMessage(string.Empty));

                    // Restart timers and reset batch settings:                    
                    cosmosTimer.Restart();
                    _cosmosElapsedTime = 0;
                    _cosmosRUsPerBatch = 0;
                    _cosmosRequestsSucceededInBatch = 0;

                    // Output all messages available right now, in one go.
                    progress.Report(ProgressWithMessages(ConsumeAsEnumerable(messages)));
                }
            }

            messages.Enqueue(new ColoredMessage("Data generation complete", Color.Magenta));
            progress.Report(ProgressWithMessages(ConsumeAsEnumerable(messages)));

            BulkheadForCosmosDbCalls.Dispose();
            cosmosTimer.Stop();
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Extracts properties from either the appsettings.json file or system environment variables.
        /// </summary>
        /// <returns>
        /// CosmosDbConnectionString: The primary or secondary connection string copied from your Cosmos DB properties.
        /// MillisecondsToRun: The maximum amount of time to allow the generator to run before stopping transmission of data. The default value is 1800 (30 minutes).
        /// MillisecondsToLead: The amount of time to wait before sending tweet data. Default value is 0.
        /// </returns>
        private static (string CosmosDbConnectionString, int MillisecondsToRun, int MillisecondsToLead) ParseArguments()
        {
            try
            {
                // The Configuration object will extract values either from the machine's environment variables, or the appsettings.json file.
                var cosmosDbEndpointUrl = _configuration["COSMOS_DB_CONNECTION_STRING"];
                var numberOfMillisecondsToRun = (int.TryParse(_configuration["SECONDS_TO_RUN"], out var outputSecondToRun) ? outputSecondToRun : 0) * 1000;
                var numberOfMillisecondsToLead = (int.TryParse(_configuration["SECONDS_TO_LEAD"], out var outputSecondsToLead) ? outputSecondsToLead : 0) * 1000;

                if (string.IsNullOrWhiteSpace(cosmosDbEndpointUrl))
                {
                    throw new ArgumentException("COSMOS_DB_CONNECTION_STRING must be provided");
                }

                return (cosmosDbEndpointUrl, numberOfMillisecondsToRun, numberOfMillisecondsToLead);
            }
            catch (Exception e)
            {
                WriteLineInColor(e.Message, ConsoleColor.Red);
                Console.ReadLine();
                throw;
            }
        }

        #endregion

        #region Cosmos DB

        private static async Task InitializeCosmosDb()
        {
            await _cosmosDbClient.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });

            // We create a partitioned collection here which needs a partition key. Partitioned collections
            // can be created with very high values of provisioned throughput (up to OfferThroughput = 250,000)
            // and used to store up to 250 GB of data.
            var collectionDefinition = new DocumentCollection { Id = CollectionName };

            // Create a partition based on the ipCountryCode value from the Transactions data set.
            // This partition was selected because the data will most likely include this value, and
            // it allows us to partition by location from which the transaction originated. This field
            // also contains a wide range of values, which is preferable for partitions.
            collectionDefinition.PartitionKey.Paths.Add($"/{PartitionKey}");

            // Use the recommended indexing policy which supports range queries/sorting on strings.
            collectionDefinition.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

            // Create with a throughput of 15000 RU/s.
            await _cosmosDbClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DatabaseName),
                collectionDefinition,
                new RequestOptions { OfferThroughput = 15000 });
        }

        /// <summary>
        /// Get the database if it exists, null if it doesn't.
        /// </summary>
        /// <returns>The requested database</returns>
        private static Database GetDatabaseIfExists(string databaseName)
        {
            return _cosmosDbClient.CreateDatabaseQuery().Where(d => d.Id == databaseName).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Get the collection if it exists, null if it doesn't.
        /// </summary>
        /// <returns>The requested collection</returns>
        private static DocumentCollection GetCollectionIfExists(string databaseName, string collectionName)
        {
            if (GetDatabaseIfExists(databaseName) == null)
            {
                return null;
            }

            return _cosmosDbClient.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(databaseName))
                .Where(c => c.Id == collectionName).AsEnumerable().FirstOrDefault();
        }

        #endregion

        #region Console Output Formatting

        private static void OutputStatistics(Statistic[] statistics)
        {
            if (!statistics.Any()) return;
            // Output statistics.
            var longestDescription = statistics.Max(s => s.Description.Length);
            foreach (var stat in statistics)
            {
                WriteLineInColor(stat.Description.PadRight(longestDescription) + ": " + stat.Value, stat.Color.ToConsoleColor());
            }
        }

        public static Progress ProgressWithMessages(IEnumerable<ColoredMessage> messages)
        {
            return new Progress(LatestStatistics, messages);
        }

        public static void WriteLineInColor(string msg, ConsoleColor color)
        {
            lock (LockObject)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ResetColor();
            }
        }

        public static IEnumerable<T> ConsumeAsEnumerable<T>(ConcurrentQueue<T> concurrentQueue)
        {
            while (concurrentQueue.TryDequeue(out T got))
            {
                yield return got;
            }
        }

        #endregion
    }
}
