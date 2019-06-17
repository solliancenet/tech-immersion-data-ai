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
                    Console.WriteLine("PipelineEnhancer creates and updates Cognitive Search pipelines.");
                    Console.WriteLine("=============");
                    Console.WriteLine("** Enter 1 to add a Sentiment Analysis cognitive skill to the Tweets search index.");
                    //Console.WriteLine("** Enter 2 to include Personalized ranking information in the Tweets search index.");
                    Console.WriteLine("** Enter 2 to integrate a custom text translator skill to the Tweets search index.");
                    Console.WriteLine("** Enter 3 to create a new search pipeline for searching and recognizing forms in Blob Storage.");
                    Console.WriteLine("** Enter 4 to create a new search pipeline for indexing vehicle telemetry and inspecting for engine temperature anomalies.");
                    Console.WriteLine("=============");
                    Console.WriteLine("** Enter X to exit the console application.");
                    Console.WriteLine("=============");
                    Console.WriteLine("");

                    var userInput = "";

                    while (true)
                    {
                        Console.Write("Enter the number of the operation you would like to perform > ");

                        var input = Console.ReadLine();
                        if (input.Equals("1", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("2", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("3", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("4", StringComparison.InvariantCultureIgnoreCase) ||
                            //input.Equals("5", StringComparison.InvariantCultureIgnoreCase) ||
                            input.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                        {
                            userInput = input.Trim();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input entered. Please enter a number between 1 and 4, or X.");
                        }
                    }

                    if (userInput.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }

                    try
                    {

                        // Retrieve the components from the API, or create base objects if they don't exist.
                        var index = await CognitiveSearchHelper.GetIndex(searchClient, appConfig.Search.IndexName);
                        var indexer = await CognitiveSearchHelper.GetIndexer(searchClient, appConfig.Search);
                        var skillset = await CognitiveSearchHelper.GetSkillset(searchClient, appConfig.Search.SkillsetName, appConfig.CognitiveServices);
                        var message = "";

                        Console.WriteLine("");
                        
                        switch(userInput)
                        {
                            case "4":
                                await CreateAnomalyDetectionPipeline(searchClient, appConfig);
                                continue;
                            case "3":
                                var modelId = await TrainFormRecognizerModel(appConfig.FormRecognizer, appConfig.BlobStorage);
                                await CreateFormsRecognitionPipeline(searchClient, appConfig, modelId);
                                continue;
                            case "2":
                                AddCustomTranslateSkill(ref index, ref indexer, ref skillset, appConfig.FunctionApp);
                                Console.WriteLine("Your custom translator skill was successfully integrated to the search pipeline.");
                                goto case "1";
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
                                index = await CognitiveSearchHelper.GetIndexFromFile(appConfig.Search.IndexName);
                                indexer = await CognitiveSearchHelper.GetIndexerFromFile(appConfig.Search);
                                skillset = await CognitiveSearchHelper.GetSkillsetFromFile(appConfig.Search.SkillsetName, appConfig.CognitiveServices);
                                message = "The search pipeline has been restored to its initial state";
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

            index.Fields.Add(new Field("sentiment", DataType.Double));
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping("document/sentiment", "sentiment").GetAwaiter().GetResult());
            skillset.Skills.Add(new SentimentSkill
            {
                Context = "/document",
                Description = "Sentiment analysis skill",
                DefaultLanguageCode = "en",
                Inputs = new List<InputFieldMappingEntry> { new InputFieldMappingEntry("text", "/document/text") },
                Outputs = new List<OutputFieldMappingEntry> { new OutputFieldMappingEntry("score", "sentiment") }
            });
        }

        #endregion

        #region Task 2: Add Personalizer fields

        #endregion

        #region Task 2: Add user fields

        private static void AddUserInfoToIndex(ref Index index, ref Indexer indexer)
        {
            var analyzer = AnalyzerName.StandardLucene;
            // Create a new index fields for userName and userLocation
            index.Fields.Add(new Field("userName", analyzer));
            index.Fields.Add(new Field("userLocation", analyzer));

            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping("/document/user/name", "userName").GetAwaiter().GetResult());
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping("/document/user/location", "userLocation").GetAwaiter().GetResult());
        }

        #endregion

        #region Task 3: Integrate custom translator skill

        private static void AddCustomTranslateSkill(ref Index index, ref Indexer indexer, ref Skillset skillset, FunctionAppConfig config)
        {
            var targetField = "textTranslated";
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            index.Fields.Add(new Field(targetField, AnalyzerName.StandardLucene));
            indexer.OutputFieldMappings.Add(CognitiveSearchHelper.CreateFieldMapping($"/document/{targetField}", targetField).GetAwaiter().GetResult());

            // Create the custom translate skill
            skillset.Skills.Add(new WebApiSkill
            {
                Description = "Custom translator skill",
                Context = "/document",
                Uri = $"{config.Url}/api/Translate?code={config.DefaultHostKey}",
                HttpMethod = "POST",
                //HttpHeaders = new WebApiHttpHeaders(headers),
                BatchSize = 1,
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("text", "/document/text"),
                    new InputFieldMappingEntry("language", "/document/language")
                },
                Outputs = new List<OutputFieldMappingEntry>
                {
                    new OutputFieldMappingEntry("text", targetField)
                }
            });
            
            // Update all the other skills, except for the LanguageDetectionSkill, to use the new textTranslated field.
            foreach (var skill in skillset.Skills)
            {
                var type = skill.GetType();
                var typeName = type.Name;

                if (typeName != "WebApiSkill" && typeName != "LanguageDetectionSkill")
                {
                    foreach (var input in skill.Inputs)
                    {
                        if (input.Source == "/document/text")
                        {
                            input.Source = $"/document/{targetField}";
                        }
                    }
                }
            }
        }

        #endregion

        #region Task 4: Form Recognizer skill

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

            Console.WriteLine("");
            Console.WriteLine("");
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
            var headers = new WebApiHttpHeaders(new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            });

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

        #region Task 7: Anomaly detection skill

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

            Console.WriteLine("");
            Console.WriteLine("");
        }

        private static void AddCustomAnomalyDetectorSkill(ref Index index, ref Indexer indexer, ref Skillset skillset, AppConfig config)
        {
            var headers = new WebApiHttpHeaders(new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            });

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

            // Send the training request to the Anomaly Detector batch detect enpoint.
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