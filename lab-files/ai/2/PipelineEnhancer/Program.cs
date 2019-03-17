using Microsoft.Extensions.Configuration;
using Search;
using Search.Common.Extensions;
using Search.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace PipelineEnhancer
{
    internal class Program
    {
        private const string TweetDataSource = "tweets-cosmosdb";
        private const string TweetIndex = "tweet-index";
        private const string TweetIndexer = "tweet-indexer";
        private const string TweetSkillset = "tweet-skillset";

        private static IConfigurationRoot _configuration;
        private static SearchServiceClient _serviceClient;

        static void Main(string[] args)
        {
            // Setup configuration to either read from the appsettings.json file (if present) or environment variables.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var arguments = ParseArguments();

            _serviceClient = CreateSearchServiceClient(arguments.SearchServiceUrl, arguments.SearchServiceKey);

            while(true)
            {
                Console.WriteLine("PipelineEnhancer updates your Cognitive Search pipeline.");
                Console.WriteLine("** Enter 1 to add a sentiment analysis cognitive skill.");
                Console.WriteLine("** Enter 2 to include user information to the search index.");
                Console.WriteLine("** Enter 3 to integrate your custom skill to the pipeline.");
                Console.WriteLine("** Enter X to exit the console application.");
                Console.WriteLine("");

                var userInput = "";

                while (true)
                {
                    Console.Write("Enter the number of the operation you would like to perform > ");

                    var input = Console.ReadLine();
                    if (input.Equals("1", StringComparison.InvariantCultureIgnoreCase) ||
                        input.Equals("2", StringComparison.InvariantCultureIgnoreCase) ||
                        input.Equals("3", StringComparison.InvariantCultureIgnoreCase) ||
                        input.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                    {
                        userInput = input.Trim();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input entered. Please enter either 1, 2, or 3.");
                    }
                }

                if (userInput.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                // Use code to create the base objects. Don't assume these arlready exist, or are in the right shape.
                var index = CreateBaseIndex();
                var indexer = CreateBaseIndexer();
                var skillset = CreateBaseSkillset(arguments.CognitiveServicesResourceId, arguments.CognitiveServicesKey);

                if (userInput.Equals("1", StringComparison.InvariantCultureIgnoreCase))
                {
                    AddSentimentAnalysisSkill(ref index, ref indexer, ref skillset);
                    CreateCognitiveSearchPipeline(index, indexer, skillset);
                    Console.WriteLine("");
                    Console.WriteLine("The sentiment analysis skill was successfully added to the search pipeline.");
                }
                else if (userInput.Equals("2", StringComparison.InvariantCultureIgnoreCase))
                {
                    AddSentimentAnalysisSkill(ref index, ref indexer, ref skillset);
                    AddUserInfoToIndex(ref index, ref indexer, ref skillset);
                    CreateCognitiveSearchPipeline(index, indexer, skillset);
                    Console.WriteLine("");
                    Console.WriteLine("User information was successfully added to the search pipeline.");
                }
                else if (userInput.Equals("3", StringComparison.InvariantCultureIgnoreCase))
                {
                    AddSentimentAnalysisSkill(ref index, ref indexer, ref skillset);
                    AddUserInfoToIndex(ref index, ref indexer, ref skillset);
                    AddCustomTranslateSkill(ref index, ref indexer, ref skillset, arguments.FunctionAppUrl, arguments.FunctionAppKey);
                    CreateCognitiveSearchPipeline(index, indexer, skillset);
                    Console.WriteLine("");
                    Console.WriteLine("Your custom translator skill was successfully integrated to the search pipeline.");
                }

                Console.WriteLine("");
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Initializes a Search Service Client
        /// </summary>
        private static SearchServiceClient CreateSearchServiceClient(string searchServiceName, string searchServiceKey)
        {
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, searchServiceKey);
            return serviceClient;
        }

        /// <summary>
        /// Creates the Index, Indexer, and Skillset for the cognitive search pipeline.
        /// </summary>
        private static void CreateCognitiveSearchPipeline(Index index, Indexer indexer, Skillset skillset)
        {
            // Delete the existing Index, Indexer and Skillset
            DeleteIndexerIfExists(TweetIndexer);
            DeleteSkillsetIfExists(TweetSkillset);
            DeleteIndexIfExists(TweetIndex);

            // Create a new Index, Indexer and Skillset.
            CreateSkillset(skillset);
            CreateIndex(index);
            CreateIndexer(indexer);
        }

        #region Task 1: Add sentiment analysis

        private static void AddSentimentAnalysisSkill(ref Index index, ref Indexer indexer, ref Skillset skillset)
        {
            // Create the sentiment skill
            var sentimentSkill = new SentimentSkill
            {
                Context = "/document",
                Description = "Sentiment analysis skill",
                DefaultLanguageCode = "en",
                Inputs = new List<InputFieldMappingEntry> { new InputFieldMappingEntry("text", "/document/text") },
                Outputs = new List<OutputFieldMappingEntry> { new OutputFieldMappingEntry("score", "sentiment") }
            };
            skillset.Skills.Add(sentimentSkill);

            // Create a new index field
            var sentimentField = new Field
            {
                Name = "sentiment",
                Type = DataType.Double,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = true
            };
            index.Fields.Add(sentimentField);

            indexer.OutputFieldMappings.Add(CreateFieldMapping("document/sentiment", "sentiment"));
        }

        #endregion

        #region Task 2: Add user fields

        private static void AddUserInfoToIndex(ref Index index, ref Indexer indexer, ref Skillset skillset)
        {
            // Create a new index fields for userName and userLocation
            var userNameField = new Field
            {
                Name = "userName",
                Type = DataType.String,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = true,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(userNameField);

            var userLocationField = new Field
            {
                Name = "userLocation",
                Type = DataType.String,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = true,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(userLocationField);

            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/user/name", "userName"));
            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/user/location", "userLocation"));
        }

        #endregion

        #region Task 3: Integrate custom translator skill

        private static void AddCustomTranslateSkill(ref Index index, ref Indexer indexer, ref Skillset skillset, string functionAppUrl, string functionAppKey)
        {
            var targetField = "textTranslated";
            
            // Create the custom translate skill
            var translateSkill = new WebApiSkill
            {
                Description = "Custom translator skill",
                Context = "/document",
                Uri = $"{functionAppUrl}/api/Translate?code={functionAppKey}",
                BatchSize = 1,
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("text", "/document/text"),
                    new InputFieldMappingEntry("language", "/document/language")
                },
                Outputs = new List<OutputFieldMappingEntry> { new OutputFieldMappingEntry("text", targetField) }
            };
            skillset.Skills.Add(translateSkill);
            

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

            // Create a new index field
            var sentimentField = new Field
            {
                Name = targetField,
                Type = DataType.String,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = true,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(sentimentField);

            indexer.OutputFieldMappings.Add(CreateFieldMapping($"/document/{targetField}", targetField));
        }

        #endregion

        #region Index methods

        private static Index CreateBaseIndex()
        {
            var index = new Index { Name = TweetIndex };
            index.Fields = new List<Field>();

            // Add base index fields
            var createdAtField = new Field
            {
                Name = "created_at",
                Type = DataType.String,
                IsSortable = true,
                IsKey = false
            };
            index.Fields.Add(createdAtField);

            var idStrField = new Field
            {
                Name = "id_str",
                Type = DataType.String,
                IsSearchable = true,
                IsRetrievable = true,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(idStrField);

            var idField = new Field
            {
                Name = "id",
                IsKey = false,
                Type = DataType.String
            };
            index.Fields.Add(idField);

            var textField = new Field
            {
                Name = "text",
                Type = DataType.String,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = true,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(textField);

            var ridField = new Field
            {
                Name = "rid",
                Type = DataType.String,
                IsKey = true,
                IsRetrievable = true
            };
            index.Fields.Add(ridField);

            var peopleField = new Field
            {
                Name = "people",
                Type = DataType.StringCollection,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = false,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(peopleField);

            var organizationsField = new Field
            {
                Name = "organizations",
                Type = DataType.StringCollection,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = false,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(organizationsField);

            var locationsField = new Field
            {
                Name = "locations",
                Type = DataType.StringCollection,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = false,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(locationsField);

            var keyPhrasesField = new Field
            {
                Name = "keyphrases",
                Type = DataType.StringCollection,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = false,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(keyPhrasesField);

            var languageField = new Field
            {
                Name = "language",
                Type = DataType.String,
                IsSearchable = true,
                IsFilterable = true,
                IsRetrievable = true,
                IsSortable = true,
                IsKey = false,
                Analyzer = AnalyzerName.StandardLucene
            };
            index.Fields.Add(languageField);

            return index;
        }

        private static Index CreateIndex(Index index)
        {
            return _serviceClient.CreateIndex(index).GetAwaiter().GetResult();
        }

        private static void DeleteIndexIfExists(string indexName)
        {
            if (_serviceClient.Indexes.Exists(indexName))
            {
                _serviceClient.DeleteIndex(indexName).GetAwaiter().GetResult();
            }
        }

        private static Index GetIndexIfExists(string indexName)
        {
            if (_serviceClient.Indexes.Exists(indexName))
            {
                return _serviceClient.Indexes.Get(indexName);
            }

            return null;
        }

        #endregion

        #region Indexer methods

        private static Indexer CreateBaseIndexer()
        {
            var indexer = new Indexer
            {
                Name = TweetIndexer,
                Description = "Tweet indexer",
                DataSourceName = TweetDataSource,
                SkillsetName = TweetSkillset,
                TargetIndexName = TweetIndex,
                Schedule = new IndexingSchedule()
            };
            indexer.OutputFieldMappings = new List<FieldMapping>();

            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/people", "people"));
            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/organizations", "organizations"));
            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/locations", "locations"));
            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/keyphrases", "keyphrases"));
            indexer.OutputFieldMappings.Add(CreateFieldMapping("/document/language", "language"));

            return indexer;
        }

        private static FieldMapping CreateFieldMapping(string sourceFieldName, string targetFieldName)
        {
            return new FieldMapping
            {
                SourceFieldName = sourceFieldName,
                TargetFieldName = targetFieldName
            };
        }

        private static Indexer CreateIndexer(Indexer indexer)
        {
            return _serviceClient.CreateIndexer(indexer).GetAwaiter().GetResult();
        }

        private static void DeleteIndexerIfExists(string indexerName)
        {
            if (_serviceClient.Indexers.Exists(indexerName))
            {
                _serviceClient.DeleteIndexer(indexerName).GetAwaiter().GetResult();
            }
        }

        private static Indexer GetIndexerIfExists(string indexerName)
        {
            if (_serviceClient.Indexers.Exists(indexerName))
            {
                return _serviceClient.Indexers.Get(indexerName);
            }

            return null;
        }

        #endregion

        #region Skillset methods

        private static Skillset CreateBaseSkillset(string cognitiveServicesResourceId, string cognitiveServicesKey)
        {
            var skillset = new Skillset
            {
                Name = TweetSkillset,
                Description = "Cognitive skills collection"
            };
            skillset.Skills = new List<Skill>();

            var entityRecognitionSkill = new EntityRecognitionSkill
            {
                Description = "Entity recognition skill",
                Context = "/document",
                Categories = new List<string>
                {
                    EntityCategory.Person,
                    EntityCategory.Quantity,
                    EntityCategory.Organization,
                    EntityCategory.Location,
                    EntityCategory.DateTime,
                    EntityCategory.URL,
                    EntityCategory.Email
                },
                DefaultLanguageCode = "en",
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("text", "/document/text")
                },
                Outputs = new List<OutputFieldMappingEntry> {
                    new OutputFieldMappingEntry("persons", "people"),
                    new OutputFieldMappingEntry("organizations", "organizations"),
                    new OutputFieldMappingEntry("locations", "locations")
                }
            };
            skillset.Skills.Add(entityRecognitionSkill);

            var keyPhraseExtractionSkill = new KeyPhraseExtractionSkill
            {
                Context = "/document",
                Description = "Key phrase extraction skill",
                DefaultLanguageCode = "en",
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("text", "/document/text")
                },
                Outputs = new List<OutputFieldMappingEntry> {
                    new OutputFieldMappingEntry("keyPhrases", "keyphrases")
                }
            };
            skillset.Skills.Add(keyPhraseExtractionSkill);

            var languageDetectionSkill = new LanguageDetectionSkill
            {
                Context = "/document",
                Description = "Language detection skill",
                Inputs = new List<InputFieldMappingEntry>
                {
                    new InputFieldMappingEntry("text", "/document/text")
                },
                Outputs = new List<OutputFieldMappingEntry> {
                    new OutputFieldMappingEntry("languageCode", "language")
                }
            };
            skillset.Skills.Add(languageDetectionSkill);

            skillset.CognitiveServices = new CognitiveServices(cognitiveServicesResourceId, cognitiveServicesKey);

            return skillset;
        }

        private static Skillset CreateSkillset(Skillset skillset)
        {

            return _serviceClient.CreateSkillset(skillset).GetAwaiter().GetResult();
        }

        private static void DeleteSkillsetIfExists(string skillsetName)
        {
            if (_serviceClient.Skillsets.Exists(skillsetName))
            {
                _serviceClient.DeleteSkillset(skillsetName).GetAwaiter().GetResult();
            }
        }

        private static Skillset GetSkillsetIfExists(string skillsetName)
        {
            if (_serviceClient.Skillsets.Exists(skillsetName))
            {
                return _serviceClient.Skillsets.Get(skillsetName);
            }

            return null;
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Extracts properties from either the appsettings.json file or system environment variables.
        /// </summary>
        private static (string SearchServiceUrl, string SearchServiceKey, string CognitiveServicesResourceId, string CognitiveServicesKey, string FunctionAppUrl, string FunctionAppKey) ParseArguments()
        {
            try
            {
                // The Configuration object will extract values either from the machine's environment variables, or the appsettings.json file.
                var searchServiceUrl = _configuration["AZURE_SEARCH_SERVICE_URL"];
                var searchServiceKey = _configuration["AZURE_SEARCH_SERVICE_KEY"];
                var cognitiveServicesResourceId = _configuration["COGNITIVE_SERVICES_RESOURCE_ID"];
                var cognitiveServicesKey = _configuration["COGNITIVE_SERVICES_KEY"];
                var functionAppUrl = _configuration["AZURE_FUNCTION_APP_URL"];
                var functionAppKey = _configuration["AZURE_FUNCTION_APP_DEFAULT_HOST_KEY"];

                if (string.IsNullOrWhiteSpace(searchServiceUrl))
                {
                    throw new ArgumentException("AZURE_SEARCH_SERVICE_URL must be provided");
                }

                return (searchServiceUrl, searchServiceKey, cognitiveServicesResourceId, cognitiveServicesKey, functionAppUrl, functionAppKey);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        #endregion
    }
}