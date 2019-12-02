using CosmosDb.Common;
using Documents = Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PipelineEnhancer.Configuration;
using PipelineEnhancer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PipelineEnhancer
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;

        static async Task Main(string[] args)
        {
            // Setup configuration to read from the appsettings.json file.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            var appConfig = new AppConfig();
            _configuration.Bind(appConfig);

            using (var searchClient = new SearchServiceClient(appConfig.Search.ServiceName, new SearchCredentials(appConfig.Search.Key)))
            {
                while (true)
                {
                    Console.ResetColor();
                    Console.WriteLine("=============");
                    Console.WriteLine("Choose an option below to run the PipelineEnhancer.");
                    Console.WriteLine("** Enter 1 to add a Sentiment Analysis cognitive skill to the Tweets search index.");
                    Console.WriteLine("** Enter 2 to add a knowledge store.");
                    Console.WriteLine("** Enter 3 to create a new search pipeline for searching and recognizing forms in Blob Storage.");
                    Console.WriteLine("** Enter 4 to create a new search pipeline for indexing vehicle telemetry and inspecting for engine temperature anomalies.");
                    Console.WriteLine("** Enter X to exit the console application.");
                    Console.WriteLine("=============");
                    Console.WriteLine("");

                    var userInput = "";

                    while (true)
                    {
                        Console.Write("Enter the number of the operation you would like to perform > ");

                        var input = Console.ReadLine().Trim();
                        if (input.Equals("1", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("2", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("3", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("4", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                        {
                            userInput = input;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input entered. Please enter a number between 1 and 6, or X.");
                        }
                    }

                    if (userInput.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }

                    try
                    {
                        // Set the components back to their base objects, from JSON files
                        var index = await CognitiveSearchHelper.GetIndexFromFile(appConfig.Search.IndexName);
                        var indexer = await CognitiveSearchHelper.GetIndexerFromFile(appConfig.Search);
                        var skillset = await CognitiveSearchHelper.GetSkillsetFromFile(appConfig.Search.SkillsetName, appConfig.CognitiveServices);

                        var message = "";

                        Console.WriteLine("");
                        
                        switch(userInput)
                        {
                            case "4":
                                await CreateAnomalyDetectionPipeline(searchClient, appConfig);
                                break;
                            case "3":
                                var modelId = await TrainFormRecognizerModel(appConfig.FormRecognizer, appConfig.BlobStorage);
                                await CreateFormsRecognitionPipeline(searchClient, appConfig, modelId);
                                break;
                            case "2":
                                AddKnowledgeStore(searchClient, appConfig, ref index, ref indexer, ref skillset);
                                Console.WriteLine("Successfully added the knowledge store to the cognitive search pipeline.");
                                break;
                            case "1":
                                AddSentimentAnalysisSkill(ref index, ref indexer, ref skillset);
                                message = "The sentiment analysis skill was successfully added to the search pipeline.";
                                await CognitiveSearchHelper.CreateCognitiveSearchPipeline(searchClient, appConfig.Search, index, indexer, skillset)
                                    .ContinueWith(t =>
                                    {
                                        Console.WriteLine(t.IsFaulted
                                            ? t.Exception.Message
                                            : message);
                                    });
                                break;
                            default:
                                message = "Resetting the search pipeline to its initial state...";
                                await CognitiveSearchHelper.CreateCognitiveSearchPipeline(searchClient, appConfig.Search, index, indexer, skillset)
                                    .ContinueWith(t =>
                                    {
                                        Console.WriteLine(t.IsFaulted
                                            ? t.Exception.Message
                                            : message);
                                    });
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            }
        }

        #region Task 1: Add sentiment analysis

        private static void AddSentimentAnalysisSkill(ref Index index, ref Indexer indexer, ref Skillset skillset)
        {
            if (index.Fields.Any(f => f.Name == "sentiment"))
            {
                return;
            }

            var sentimentField = new Field("sentiment", DataType.Double)
            {
                IsSortable = true,
                IsFilterable = true
            };
            index.Fields.Add(sentimentField);
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping("document/sentiment", "sentiment").GetAwaiter().GetResult());
            skillset.Skills.Add(new SentimentSkill
            {
                Context = "/document",
                Description = "Sentiment analysis skill",
                DefaultLanguageCode = SentimentSkillLanguage.En,
                Inputs = new List<InputFieldMappingEntry> { new InputFieldMappingEntry("text", "/document/text") },
                Outputs = new List<OutputFieldMappingEntry> { new OutputFieldMappingEntry("score", "sentiment") }
            });
        }

        #endregion

        #region Task 2: Add knowledge store

        private static void AddKnowledgeStore(SearchServiceClient searchClient, AppConfig appConfig, ref Index index, ref Indexer indexer, ref Skillset skillset)
        {
            // Add the skills from previous steps
            AddSentimentAnalysisSkill(ref index, ref indexer, ref skillset);

            // Convert the Skillset into a JSON string.
            var skillsetJson = CognitiveSearchHelper.GetSkillsetJson(skillset).GetAwaiter().GetResult();

            //  Insert ShaperSkill into the Skillset as a JSON string
            Console.WriteLine("Adding Shaper Skill to the pipeline to set up the table projections.");
            var shaperSkillJson = GetJsonFromFile("shaper-skill").GetAwaiter().GetResult();
            var skillsetJsonWithShaperSkill = CognitiveSearchHelper.InsertSkillAsJson(skillsetJson, shaperSkillJson);

            // Insert knowledge store JSON string into the Skillset JSON
            Console.WriteLine("Inserting the knowledge store with table projections.");
            var updatedSkillset = InsertKnowledgeStoreJson(skillsetJsonWithShaperSkill, appConfig.BlobStorage);

            // Create the search pipeline using the updated skillset JSON. No SDK exists yet for doing this using the SDK objects, so must use the REST API to accomplish adding a knowledge store via code.
            Console.WriteLine("Rebuilding cognitive search pipeline...");
            CognitiveSearchHelper.CreateCognitiveSearchPipeline(searchClient, appConfig.Search, index, indexer, skillset.Name, updatedSkillset).GetAwaiter().GetResult();
        }

        private static string InsertKnowledgeStoreJson(string skillsetJson, BlobStorageConfig storageConfig)
        {
            var knowledgeStoreJson = GetJsonFromFile("knowledge-store").GetAwaiter().GetResult();
            knowledgeStoreJson = knowledgeStoreJson.Replace("[storage-connection-string]", storageConfig.ConnectionString);

            var skillset = JObject.Parse(skillsetJson);
            var jtoken = JToken.Parse(knowledgeStoreJson);
            skillset.Add("knowledgeStore", jtoken);
            return skillset.ToString();
        }

        private static async Task<string> GetJsonFromFile(string fileName)
        {
            using (var reader = new StreamReader($"PipelineJson/{fileName}.json"))
            {
                var json = await reader.ReadToEndAsync();
                return json;
            }
        }

        #endregion

        #region Task 3: Form Recognizer skill

        private static async Task CreateFormsRecognitionPipeline(SearchServiceClient searchClient, AppConfig appConfig, string modelId)
        {
            var formsSearchConfig = new SearchConfig
            {
                DataSourceName = "forms-datasource",
                IndexName = "forms-index",
                IndexerName = "forms-indexer",
                SkillsetName = "forms-skillset"
            };

            var formsDataSource = await CognitiveSearchHelper.GetOrCreateBlobDataSource(searchClient, formsSearchConfig.DataSourceName, DataSourceType.AzureBlob, appConfig.BlobStorage);
            Console.WriteLine($"Successfully created data source {formsSearchConfig.DataSourceName}");

            var formsIndex = await CognitiveSearchHelper.GetIndexFromFile(formsSearchConfig.IndexName);
            var formsIndexer = await CognitiveSearchHelper.GetIndexerFromFile(formsSearchConfig);
            var formsSkillset = await CognitiveSearchHelper.GetSkillsetFromFile(formsSearchConfig.SkillsetName, appConfig.CognitiveServices);

            Console.WriteLine("Adding Custom Form Recognizer skill to pipeline");
            AddCustomFormRecognizerSkill(ref formsIndex, ref formsIndexer, ref formsSkillset, appConfig, modelId);

            await CognitiveSearchHelper.CreateCognitiveSearchPipeline(searchClient, appConfig.Search, formsIndex, formsIndexer, formsSkillset)
                .ContinueWith(t =>
                {
                    Console.WriteLine(t.IsFaulted
                        ? t.Exception.Message
                        : "Your forms recognizer pipeline was successfully created.");
                });
        }

        /// <summary>
        /// Uses the documents contained in the target Blob Storage account to train the Form Recognizer model.
        /// </summary>
        private static async Task<string> TrainFormRecognizerModel(FormRecognizerConfig formConfig, BlobStorageConfig storageConfig)
        {
            var formRecognizerTrainUri = $"{formConfig.Endpoint}formrecognizer/v1.0-preview/custom/train";
            var sasUri = $"https://{storageConfig.AccountName}.blob.core.windows.net/{storageConfig.ContainerName}/{storageConfig.SasToken}";
            var requestBody = JsonConvert.SerializeObject(new FormRecognizerTrainRequestBody { Source = sasUri });

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(formRecognizerTrainUri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", formConfig.Key);

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if(!response.IsSuccessStatusCode)
                    {
                        var errorResponse = JsonConvert.DeserializeObject<FormRecognizerErrorResponse>(responseBody);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(errorResponse.Error.Message);
                        return "";
                    }

                    var successResponse = JsonConvert.DeserializeObject<FormRecognizerTrainSuccessResponse>(responseBody);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Successfully trained the form recognizer model with {successResponse.TrainingDocuments.Count} forms.");
                    Console.ResetColor();
                    
                    return successResponse.ModelId;
                }
            }
        }

        private static void AddCustomFormRecognizerSkill(ref Index index, ref Indexer indexer, ref Skillset skillset, AppConfig config, string modelId)
        {
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            index.Fields.Add(new Field($"formHeight", DataType.Int32));
            index.Fields.Add(new Field($"formWidth", DataType.Int32));
            index.Fields.Add(new Field($"formKeyValuePairs", DataType.Collection(DataType.String)));
            index.Fields.Add(new Field($"formColumns", DataType.Collection(DataType.String)));
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping($"/document/formHeight", "formHeight").GetAwaiter().GetResult());
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping($"/document/formWidth", "formWidth").GetAwaiter().GetResult());
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping($"/document/formKeyValuePairs", "formKeyValuePairs").GetAwaiter().GetResult());
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping($"/document/formColumns", "formColumns").GetAwaiter().GetResult());

            // Create the custom translate skill
            skillset.Skills.Add(new WebApiSkill
            {
                Description = "Custom Form Recognizer skill",
                Context = "/document",
                Uri = $"{config.FunctionApp.Url}/api/AnalyzeForm?code={config.FunctionApp.DefaultHostKey}&modelId={modelId}",
                HttpMethod = "POST",
                //HttpHeaders = new WebApiHttpHeaders(), // This is broken in the SDK, so handle by sending JSON directly to Rest API.
                BatchSize = 1,
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("contentType", "/document/fileContentType"),
                    new InputFieldMappingEntry("storageUri", "/document/storageUri"),
                    new InputFieldMappingEntry("storageSasToken", "/document/sasToken")
                },
                Outputs = new List<OutputFieldMappingEntry>
                {
                    new OutputFieldMappingEntry("formHeight", "formHeight"),
                    new OutputFieldMappingEntry("formWidth", "formWidth"),
                    new OutputFieldMappingEntry("formKeyValuePairs", "formKeyValuePairs"),
                    new OutputFieldMappingEntry("formColumns", "formColumns"),

                }
            });
        }

        #endregion

        #region Task 4: Anomaly detection skill

        private static async Task CreateAnomalyDetectionPipeline(SearchServiceClient searchClient, AppConfig appConfig)
        {
            var searchConfig = new SearchConfig
            {
                DataSourceName = "telemetry-datasource",
                IndexName = "telemetry-index",
                IndexerName = "telemetry-indexer",
                SkillsetName = "telemetry-skillset"
            };

            var dataSource = await CognitiveSearchHelper.GetOrCreateCosmosDataSource(searchClient, searchConfig.DataSourceName, DataSourceType.CosmosDb, appConfig.CosmosDb);
            Console.WriteLine($"Successfully created data source {searchConfig.DataSourceName}");

            var index = await CognitiveSearchHelper.GetIndexFromFile(searchConfig.IndexName);
            var indexer = await CognitiveSearchHelper.GetIndexerFromFile(searchConfig);
            var skillset = new Skillset
            {
                Name = searchConfig.SkillsetName,
                Description = "Anomaly detection skills",
                CognitiveServices = new CognitiveServicesByKey(appConfig.CognitiveServices.Key, appConfig.CognitiveServices.ResourceId),
                Skills = new List<Skill>()
            };

            Console.WriteLine("Adding Custom Anomaly Detector skill to pipeline");
            AddCustomAnomalyDetectorSkill(ref index, ref indexer, ref skillset, appConfig);

            await CognitiveSearchHelper.CreateCognitiveSearchPipeline(searchClient, appConfig.Search, index, indexer, skillset)
                .ContinueWith(t =>
                {
                    Console.WriteLine(t.IsFaulted
                        ? t.Exception.Message
                        : "Your anomaly detection pipeline was successfully created.");
                });
        }

        private static void AddCustomAnomalyDetectorSkill(ref Index index, ref Indexer indexer, ref Skillset skillset, AppConfig config)
        {
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            var anomalyFields = new List<Field>
            {
                new Field($"isAnomaly", DataType.Boolean),
                new Field($"isPositiveAnomaly", DataType.Boolean),
                new Field($"isNegativeAnomaly", DataType.Boolean),
                new Field($"expectedValue", DataType.Double),
                new Field($"upperMargin", DataType.Double),
                new Field($"lowerMargin", DataType.Double)
            };
            index.Fields.Add(new Field("engineTemperatureAnalysis", DataType.Complex, anomalyFields));

            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping($"/document/engineTemperatureAnalysis", "engineTemperatureAnalysis").GetAwaiter().GetResult());

            // Create the custom translate skill
            skillset.Skills.Add(new WebApiSkill
            {
                Description = "Custom Anomaly Detector skill",
                Context = "/document",
                Uri = $"{config.FunctionApp.Url}/api/DetectAnomalies?code={config.FunctionApp.DefaultHostKey}",
                HttpMethod = "POST",
                //HttpHeaders = new WebApiHttpHeaders(), // This is broken in the SDK, so handle by sending JSON directly to Rest API.
                BatchSize = 1,
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("timestamp", "/document/timestamp"),
                    new InputFieldMappingEntry("engineTemperature", "/document/engineTemperature")
                },
                Outputs = new List<OutputFieldMappingEntry>
                {
                    new OutputFieldMappingEntry("anomalyResult", "engineTemperatureAnalysis")

                }
            });
        }

        /// <summary>
        /// Uses the vehicle telemetry contained in Cosmos DB to train the Anomaly Detector model.
        /// </summary>
        private static async Task<bool> TrainAnomalyDetectorModel(AppConfig appConfig)
        {
            var anomalyDetectorTrainUri = $"{appConfig.AnomalyDetector.Endpoint}anomalydetector/v1.0/timeseries/entire/detect";
            
            var timeSeriesData = new Series
            {
                maxAnomalyRatio = 0.25F,
                sensitivity = 95,
                granularity = "minutely"
            };

            // Retrieve data from Cosmos DB
            var cosmosDbConnectionString = new CosmosDbConnectionString(appConfig.CosmosDb.ConnectionString);
            // Set the Cosmos DB connection policy.
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };
            using (var cosmosClient = new DocumentClient(cosmosDbConnectionString.ServiceEndpoint, cosmosDbConnectionString.AuthKey, connectionPolicy))
            {
                var containerUri = UriFactory.CreateDocumentCollectionUri(appConfig.CosmosDb.DatabaseId, appConfig.CosmosDb.ContainerId);
                var query = cosmosClient.CreateDocumentQuery<EngineTempRecord>(containerUri.ToString(),
                    new Documents.SqlQuerySpec("SELECT TOP 5000 c.engineTemperature FROM c"),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                    .ToList();

                var models = new List<AnomalyModel>();
                var startDateTime = DateTime.Now.AddDays(-6);
                var i = 0;
                foreach(var temp in query)
                {
                    models.Add(new AnomalyModel { timestamp = startDateTime.AddMinutes(i), value = temp.engineTemperature });
                    i++;
                }
                timeSeriesData.series = models;
            }

            var requestBody = JsonConvert.SerializeObject(timeSeriesData);

            // Send the training request to the Anomaly Detector batch detect endpoint.
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(anomalyDetectorTrainUri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", appConfig.AnomalyDetector.Key);

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    Console.ForegroundColor = response.IsSuccessStatusCode ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine(responseBody);
                    Console.ResetColor();

                    return response.IsSuccessStatusCode;
                }
            }
        }

        #endregion
    }
}