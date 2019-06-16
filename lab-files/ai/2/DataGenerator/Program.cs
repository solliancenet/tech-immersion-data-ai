using DataGenerator.Configuration;
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
using DataGenerator.OutputHelpers;
using CosmosDb.Common;

namespace DataGenerator
{
    internal class Program
    {
        private static DocumentClient _cosmosDbClient;
        private static IConfigurationRoot _configuration;

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

        static async Task Main(string[] args)
        {
            // Setup configuration to either read from the appsettings.json file (if present) or environment variables.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            var appConfig = new AppConfig();
            _configuration.Bind(appConfig);

            var statistics = new Statistic[0];

            var progress = new Progress<Progress>();
            progress.ProgressChanged += (sender, progressArgs) =>
            {
                foreach (var message in progressArgs.Messages)
                {
                    WriteLineInColor(message.Message, message.Color.ToConsoleColor());
                }
                statistics = progressArgs.Statistics;
            };

            while(true)
            {
                Console.WriteLine("Data Generator sends generated data into Cosmos DB.");
                Console.WriteLine("** Enter 1 to generate tweets.");
                Console.WriteLine("** Enter 2 to generate vehicle telemetry.");
                Console.WriteLine("** Enter X to exit the console application.");
                Console.WriteLine("");

                var userInput = "";

                while (true)
                {
                    Console.Write("Enter the number of the operation you would like to perform > ");

                    var input = Console.ReadLine();
                    if (input.Equals("1", StringComparison.InvariantCultureIgnoreCase) ||
                        input.Equals("2", StringComparison.InvariantCultureIgnoreCase) ||
                        input.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                    {
                        userInput = input.Trim();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input entered. Please enter either 1, 2 or X.");
                    }
                }

                ThreadPool.SetMinThreads(100, 100);

                // Set an optional timeout for the generator.
                var cancellationSource = appConfig.CosmosDb.MillisecondsToRun == 0 ? new CancellationTokenSource() : new CancellationTokenSource(appConfig.CosmosDb.MillisecondsToRun);
                var cancellationToken = cancellationSource.Token;

                // Handle Control+C or Control+Break.
                Console.CancelKeyPress += (o, e) =>
                {
                    WriteLineInColor("Stopped generator. No more data is being sent.", ConsoleColor.Yellow);
                    cancellationSource.Cancel();

                    // Allow the main thread to continue and exit...
                    WaitHandle.Set();

                    OutputStatistics(statistics);
                };

                Console.WriteLine(string.Empty);
                Console.WriteLine("======");
                WriteLineInColor("Press Ctrl+C or Ctrl+Break to cancel.", ConsoleColor.Cyan);
                Console.WriteLine(string.Empty);
                var dataType = "";

                var cosmosDbConnectionString = new CosmosDbConnectionString(appConfig.CosmosDb.ConnectionString);
                // Set the Cosmos DB connection policy.
                var connectionPolicy = new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                };

                using (_cosmosDbClient = new DocumentClient(cosmosDbConnectionString.ServiceEndpoint, cosmosDbConnectionString.AuthKey, connectionPolicy))
                {
                    switch (userInput.ToLower())
                    {
                        case "1":
                            await GenerateTweets(appConfig, cancellationToken, progress);
                            dataType = "Tweet";
                            break;
                        case "2":
                            await GenerateVehicleTelemetry(appConfig, cancellationToken, progress);
                            dataType = "Vehicle Telemetry";
                            break;
                        default:
                            // Exit the application
                            cancellationSource.Cancel();
                            return;
                    }
                }

                Console.WriteLine();
                WriteLineInColor($"Done sending generated {dataType} data", ConsoleColor.Cyan);
                Console.WriteLine();
                Console.WriteLine();

                OutputStatistics(statistics);
            }
        }

        private static async Task GenerateTweets(AppConfig appConfig, CancellationToken cancellationToken, IProgress<Progress> progress)
        {
            WriteLineInColor("Generating tweets...", ConsoleColor.White);
            Console.WriteLine("Statistics for generated data will be updated for every 60 tweets sent.");

            // Initialize the telemetry generator:
            TweetGenerator.Init();

            await InitializeCosmosDb(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.TweetsContainerId, appConfig.CosmosDb.TweetsPartitionKey);

            // Find and output the collection details, including # of RU/s.
            var dataCollection = GetContainerIfExists(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.TweetsContainerId);
            var offer = (OfferV2)_cosmosDbClient.CreateOfferQuery().Where(o => o.ResourceLink == dataCollection.SelfLink).AsEnumerable().FirstOrDefault();
            if (offer != null)
            {
                var currentCollectionThroughput = offer.Content.OfferThroughput;
                WriteLineInColor($"Found collection `{appConfig.CosmosDb.TweetsContainerId}` with {currentCollectionThroughput} RU/s ({currentCollectionThroughput} reads/second; {currentCollectionThroughput / 5} writes/second @ 1KB doc size)", ConsoleColor.Green);
            }

            // Start sending data to both Event Hubs and Cosmos DB.
            await SendData(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.TweetsContainerId, cancellationToken, progress, 60, 60, true)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine($"{t.Exception.Flatten().InnerExceptions}");
                    }
                });
        }

        private static async Task GenerateVehicleTelemetry(AppConfig appConfig, CancellationToken cancellationToken, IProgress<Progress> progress)
        {
            WriteLineInColor("Generating vehicle telemetry...", ConsoleColor.White);
            Console.WriteLine("Statistics for generated data will be updated for every 500 messages sent.");

            // Initialize the telemetry generator:
            TelemetryGenerator.Init();

            // Instantiate Cosmos DB client and start sending messages:
            InitializeCosmosDb(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.TelemetryContainerId, appConfig.CosmosDb.TelemetryPartitionKey).Wait();

            // Find and output the collection details, including # of RU/s.
            var dataCollection = GetContainerIfExists(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.TelemetryContainerId);
            var offer = (OfferV2)_cosmosDbClient.CreateOfferQuery().Where(o => o.ResourceLink == dataCollection.SelfLink).AsEnumerable().FirstOrDefault();
            if (offer != null)
            {
                var currentCollectionThroughput = offer.Content.OfferThroughput;
                WriteLineInColor($"Found collection `{appConfig.CosmosDb.TelemetryContainerId}` with {currentCollectionThroughput} RU/s ({currentCollectionThroughput} reads/second; {currentCollectionThroughput / 5} writes/second @ 1KB doc size)", ConsoleColor.Green);
            }

            // Start sending data to Cosmos DB.
            await SendData(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.TelemetryContainerId, cancellationToken, progress, 100000, 500)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine($"{t.Exception.Flatten().InnerExceptions}");
                    }
                });
        }

        #region Send Data

        private static async Task SendData(string databaseId, string containerId, CancellationToken externalCancellationToken, IProgress<Progress> progress, int maxParallelization, int sendNotificationAfter, bool isTweets = false)
        {
            // Place Cosmos DB calls into bulkhead to prevent thread starvation caused by failing or waiting calls.
            // Let any number (int.MaxValue) of calls _queue for an execution slot in the bulkhead to allow the generator to send as many calls as possible.
            BulkheadPolicy BulkheadForCosmosDbCalls = Policy.BulkheadAsync(maxParallelization, int.MaxValue);

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
            var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, containerId);

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
                    
                    #region Write to Cosmos DB

                    _cosmosRequestsMade++;
                    tasks.Add(BulkheadForCosmosDbCalls.ExecuteAsync(async ct =>
                    {
                        try
                        {
                            cosmosTimer.Start();

                            ResourceResponse<Document> response = null;

                            // Send to Cosmos DB:
                            if (isTweets)
                            {
                                response = await _cosmosDbClient
                                .CreateDocumentAsync(collectionUri, TweetGenerator.Generate())
                                .ConfigureAwait(false);
                            }
                            else
                            {
                                response = await _cosmosDbClient
                                .CreateDocumentAsync(collectionUri, TelemetryGenerator.Generate())
                                .ConfigureAwait(false);
                            }
                            

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

                    if (i == 4 && isTweets)
                    {
                        var span = TimeSpan.FromMilliseconds(2000);
                        await Task.Delay(span, externalCancellationToken);
                    }
                }

                if (_totalMessages % sendNotificationAfter == 0)
                {
                    cosmosTimer.Stop();
                    _cosmosTotalElapsedTime += _cosmosElapsedTime;
                    _cosmosRequestsSucceeded += _cosmosRequestsSucceededInBatch;

                    // Calculate RUs/second/month:
                    var ruPerSecond = (_cosmosRUsPerBatch / (_cosmosElapsedTime * .001));
                    var ruPerMonth = ruPerSecond * 86400 * 30;

                    if (!isTweets)
                    {
                        // Add delay every 500 messages that are sent.
                        await Task.Delay(5000, externalCancellationToken);
                    }

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

        ///// <summary>
        ///// Extracts properties from either the appsettings.json file or system environment variables.
        ///// </summary>
        ///// <returns>
        ///// CosmosDbConnectionString: The primary or secondary connection string copied from your Cosmos DB properties.
        ///// MillisecondsToRun: The maximum amount of time to allow the generator to run before stopping transmission of data. The default value is 600 (30 minutes).
        ///// </returns>
        //private static (string CosmosDbConnectionString, int MillisecondsToRun) ParseArguments()
        //{
        //    try
        //    {
        //        // The Configuration object will extract values either from the machine's environment variables, or the appsettings.json file.
        //        var cosmosDbEndpointUrl = _configuration["COSMOS_DB_CONNECTION_STRING"];
        //        var numberOfMillisecondsToRun = (int.TryParse(_configuration["SECONDS_TO_RUN"], out var outputSecondToRun) ? outputSecondToRun : 0) * 1000;

        //        if (string.IsNullOrWhiteSpace(cosmosDbEndpointUrl))
        //        {
        //            throw new ArgumentException("COSMOS_DB_CONNECTION_STRING must be provided");
        //        }

        //        return (cosmosDbEndpointUrl, numberOfMillisecondsToRun);
        //    }
        //    catch (Exception e)
        //    {
        //        WriteLineInColor(e.Message, ConsoleColor.Red);
        //        Console.ReadLine();
        //        throw;
        //    }
        //}

        #endregion

        #region Cosmos DB

        private static async Task InitializeCosmosDb(string databaseId, string containerId, string partitionKey, int throughput = 15000)
        {
            await _cosmosDbClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });

            // We create a partitioned collection here which needs a partition key. Partitioned collections
            // can be created with very high values of provisioned throughput (up to OfferThroughput = 250,000)
            // and used to store up to 250 GB of data.
            var collectionDefinition = new DocumentCollection { Id = containerId };

            // Create a partition based on the ipCountryCode value from the Transactions data set.
            // This partition was selected because the data will most likely include this value, and
            // it allows us to partition by location from which the transaction originated. This field
            // also contains a wide range of values, which is preferable for partitions.
            collectionDefinition.PartitionKey.Paths.Add($"/{partitionKey}");

            // Use the recommended indexing policy which supports range queries/sorting on strings.
            collectionDefinition.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

            // Create with a throughput of 1000 RU/s.
            await _cosmosDbClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(databaseId),
                collectionDefinition,
                new RequestOptions { OfferThroughput = throughput });
        }

        /// <summary>
        /// Get the database if it exists, null if it doesn't.
        /// </summary>
        /// <returns>The requested database</returns>
        private static Database GetDatabaseIfExists(string databaseId)
        {
            return _cosmosDbClient
                .CreateDatabaseQuery()
                .Where(d => d.Id == databaseId)
                .AsEnumerable()
                .FirstOrDefault();
        }

        /// <summary>
        /// Get the collection if it exists, null if it doesn't.
        /// </summary>
        /// <returns>The requested collection</returns>
        private static DocumentCollection GetContainerIfExists(string databaseId, string containerId)
        {
            if (GetDatabaseIfExists(databaseId) == null)
            {
                return null;
            }

            return _cosmosDbClient
                .CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(databaseId))
                .Where(c => c.Id == containerId)
                .AsEnumerable()
                .FirstOrDefault();
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