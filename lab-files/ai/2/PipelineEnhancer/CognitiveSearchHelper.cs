using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PipelineEnhancer.Configuration;
using PipelineEnhancer.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PipelineEnhancer
{
    public static class CognitiveSearchHelper
    {
        #region Data Source helpers

        public static async Task<DataSource> GetOrCreateBlobDataSource(ISearchServiceClient serviceClient, string name, DataSourceType dataSourceType, BlobStorageConfig blobStorageConfig, string query = "")
        {
            if (await serviceClient.DataSources.ExistsAsync(name))
            {
                return await serviceClient.DataSources.GetAsync(name);
            }

            var dataSource = new DataSource
            {
                Name = name,
                Type = dataSourceType,
                Credentials = new DataSourceCredentials(blobStorageConfig.ConnectionString),
                Container = new DataContainer(blobStorageConfig.ContainerName)
            };

            return await serviceClient.DataSources.CreateAsync(dataSource);
        }

        public static async Task<DataSource> GetOrCreateCosmosDataSource(ISearchServiceClient serviceClient, string name, DataSourceType dataSourceType, CosmosDbConfig config, string query = "")
        {
            if (await serviceClient.DataSources.ExistsAsync(name))
            {
                return await serviceClient.DataSources.GetAsync(name);
            }

            var dataSource = new DataSource
            {
                Name = name,
                Type = dataSourceType,
                Credentials = new DataSourceCredentials($"{config.ConnectionString};Database={config.DatabaseId}"),
                Container = new DataContainer(config.ContainerId, "SELECT * FROM c WHERE c._ts > @HighWaterMark ORDER BY c._ts")
            };

            return await serviceClient.DataSources.CreateAsync(dataSource);
        }

        #endregion

        #region Index helpers

        public static async Task<Index> GetIndex(ISearchServiceClient serviceClient, string indexName)
        {
            if (await serviceClient.Indexes.ExistsAsync(indexName))
            {
                return await serviceClient.Indexes.GetAsync(indexName);
            }
            else
            {
                return await GetIndexFromFile(indexName);
            }
        }

        public static async Task<Index> GetIndexFromFile(string name)
        {
            using (var reader = new StreamReader($"PipelineJson/{name}.json"))
            {
                var json = await reader.ReadToEndAsync();
                var index = JsonConvert.DeserializeObject<Index>(json);
                index.Name = name;

                return index;
            }
        }

        private static async Task<Index> CreateIndex(ISearchServiceClient serviceClient, Index index)
        {
            return await serviceClient.Indexes.CreateAsync(index);
        }

        private static async Task DeleteIndexIfExists(ISearchServiceClient serviceClient, string indexName)
        {
            if (await serviceClient.Indexes.ExistsAsync(indexName))
            {
                await serviceClient.Indexes.DeleteAsync(indexName);
            }
        }

        #endregion

        #region Indexer helpers

        public static async Task<Indexer> GetIndexer(ISearchServiceClient serviceClient, SearchConfig config)
        {
            if (await serviceClient.Indexers.ExistsAsync(config.IndexerName))
            {
                return await serviceClient.Indexers.GetAsync(config.IndexerName);
            }
            else
            {
                return await GetIndexerFromFile(config);
            }
        }

        public static async Task<Indexer> GetIndexerFromFile(SearchConfig config)
        {
            using (var reader = new StreamReader($"PipelineJson/{config.IndexerName}.json"))
            {
                var json = await reader.ReadToEndAsync();
                var indexer = JsonConvert.DeserializeObject<Indexer>(json);
                indexer.Name = config.IndexerName;
                indexer.DataSourceName = config.DataSourceName;
                indexer.SkillsetName = config.SkillsetName;
                indexer.TargetIndexName = config.IndexName;
                indexer.Description = "Search Indexer";

                return indexer;
            }
        }

        private static async Task<Indexer> CreateIndexer(ISearchServiceClient serviceClient, Indexer indexer)
        {
            return await serviceClient.Indexers.CreateAsync(indexer);
        }

        private static async Task DeleteIndexerIfExists(ISearchServiceClient serviceClient, string indexerName)
        {
            if (await serviceClient.Indexers.ExistsAsync(indexerName))
            {
                await serviceClient.Indexers.DeleteAsync(indexerName);
            }
        }

        public static async Task<FieldMapping> CreateFieldMapping(string sourceFieldName, string targetFieldName)
        {
            return await Task.FromResult(new FieldMapping
            {
                SourceFieldName = sourceFieldName,
                TargetFieldName = targetFieldName
            });
        }

        #endregion

        #region Skillset helpers

        public static async Task<Skillset> GetSkillset(ISearchServiceClient serviceClient, string skillsetName, CognitiveServicesConfig cognitiveServicesConfig)
        {
            if (await serviceClient.Skillsets.ExistsAsync(skillsetName))
            {
                // Retrieve the skillset from the Search Service.
                var skillset = await serviceClient.Skillsets.GetAsync(skillsetName);
                return skillset;
            }
            else
            {
                // Retrieve the default skillset from a JSON file.
                return await GetSkillsetFromFile(skillsetName, cognitiveServicesConfig);
            }
        }

        public static async Task<Skillset> GetSkillsetFromFile(string name, CognitiveServicesConfig cognitiveServicesConfig)
        {
            using (var reader = new StreamReader($"PipelineJson/{name}.json"))
            {
                var json = await reader.ReadToEndAsync();
                var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                serializerSettings.Converters.Add(new PolymorphicDeserializeJsonConverter<Skill>("@odata.type"));
                
                var skillset = JsonConvert.DeserializeObject<Skillset>(json, serializerSettings);
                skillset.Name = name;
                skillset.Description = "Cognitive skills collection";
                skillset.CognitiveServices = new CognitiveServicesByKey(cognitiveServicesConfig.Key, cognitiveServicesConfig.ResourceId);

                return skillset;
            }
        }

        private static async Task<Skillset> CreateSkillset(ISearchServiceClient serviceClient, Skillset skillset)
        {
            return await serviceClient.Skillsets.CreateAsync(skillset).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine(t.Exception.ToString());
                    throw new Exception("error", t.Exception);
                }

                return t.Result;
            });
        }

        /// <summary>
        /// Create a skillset by sending JSON to the REST API endpoint.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="skillset"></param>
        /// <returns></returns>
        private static async Task<Skillset> CreateSkillsetViaApi(SearchConfig config, Skillset skillset)
        {
            // This function is necessary because currently the SDK fails when trying to create 
            var uri = new Uri($"https://{config.ServiceName}.search.windows.net/skillsets/{skillset.Name}?api-version={config.ApiVersion}");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("api-key", config.Key);

                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Put;
                    var cleanPayload = await GetSkillsetJson(skillset);
                    cleanPayload = cleanPayload.Replace("\"inputs\":null", "\"inputs\":[]");
                    var content = new StringContent(cleanPayload, Encoding.UTF8, "application/json");

                    var response = await (httpClient.PutAsync(uri, content));
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Skillset>(responseContent);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                        throw new Exception(error.Error.Message);
                    }
                }
            }
        }

        private static async Task<Skillset> CreateSkillsetViaApi(SearchConfig config, string skillsetName, string skillset)
        {
            // This function is necessary because currently the SDK fails when trying to create 
            var uri = new Uri($"https://{config.ServiceName}.search.windows.net/skillsets/{skillsetName}?api-version={config.ApiVersion}");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("api-key", config.Key);

                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Put;
                    var cleanPayload = skillset.Replace("\"inputs\": null", "\"inputs\": []");
                    var content = new StringContent(cleanPayload, Encoding.UTF8, "application/json");

                    var response = await (httpClient.PutAsync(uri, content));
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Skillset>(responseContent);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                        throw new Exception(error.Error.Message);
                    }
                }
            }
        }

        public static async Task<string> GetSkillsetJson(Skillset skillset)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new PolymorphicSerializeJsonConverter<Skill>("@odata.type"));
            serializerSettings.Converters.Add(new PolymorphicSerializeJsonConverter<CognitiveServices>("@odata.type"));
            var payload = await Task.Run(() => JsonConvert.SerializeObject(skillset, serializerSettings));
            // Remove problematic values
            var cleanPayload = payload.Replace(",\"@odata.etag\":null", "").Replace("\"httpHeaders\":null,", "");

            return await Task.FromResult(cleanPayload);
        }

        public static string InsertSkillAsJson(string skillsetJson, string jsonToInsert)
        {
            var skillset = JObject.Parse(skillsetJson);
            var jtoken = JToken.Parse(jsonToInsert);

            var skills = new JArray();
            foreach(var skill in skillset["skills"])
            {
                skills.Add(skill);
            }
            skills.Add(jtoken);

            skillset["skills"] = skills;

            return skillset.ToString();
        }

        private static async Task DeleteSkillsetIfExists(ISearchServiceClient serviceClient, string skillsetName)
        {
            if (await serviceClient.Skillsets.ExistsAsync(skillsetName))
            {
                await serviceClient.Skillsets.DeleteAsync(skillsetName);
            }
        }

        #endregion

        /// <summary>
        /// Creates the Index, Indexer, and Skillset for the cognitive search pipeline.
        /// </summary>
        public static async Task CreateCognitiveSearchPipeline(ISearchServiceClient searchClient, SearchConfig searchConfig, Index index, Indexer indexer, Skillset skillset)
        {
            try
            {
                // Delete the existing Index, Indexer and Skillset
                await DeleteIndexerIfExists(searchClient, indexer.Name);
                await DeleteSkillsetIfExists(searchClient, skillset.Name);
                await DeleteIndexIfExists(searchClient, index.Name);

                // Create a new Index, Indexer and Skillset.
                // Currently, the SDK is broken in how it handles Http Headers, so if using a custom WebApiSkill, send to the REST API, and don't use the SDK service.
                if (skillset.Skills.Any(s => s.GetType().Name == "WebApiSkill"))
                {
                    await CreateSkillsetViaApi(searchConfig, skillset);
                }
                else
                {
                    await CreateSkillset(searchClient, skillset);
                }
                await CreateIndex(searchClient, index);
                await CreateIndexer(searchClient, indexer);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the Index, Indexer, and Skillset for the cognitive search pipeline.
        /// </summary>
        public static async Task CreateCognitiveSearchPipeline(ISearchServiceClient searchClient, SearchConfig searchConfig, Index index, Indexer indexer, string skillsetName, string skillset)
        {
            try
            {
                // Delete the existing Index, Indexer and Skillset
                await DeleteIndexerIfExists(searchClient, indexer.Name);
                await DeleteSkillsetIfExists(searchClient, skillsetName);
                await DeleteIndexIfExists(searchClient, index.Name);

                // Create a new Index, Indexer and Skillset.
                // Currently, there is no SDK available for including a knowledge store in a skillset, so pass in the JSON string to the API to handle this.
                await CreateSkillsetViaApi(searchConfig, skillsetName, skillset);
                await CreateIndex(searchClient, index);
                await CreateIndexer(searchClient, indexer);
            }
            catch
            {
                throw;
            }
        }
    }
}