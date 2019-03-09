using Newtonsoft.Json;
using Search.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
    public class SearchServiceClient
    {
        public readonly HttpClient client;

        public SearchServiceClient(string searchServiceUrl, string searchServiceKey)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{searchServiceUrl}/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("api-key", searchServiceKey);
        }

        public string ApiVersion { get; set; } = "2017-11-11-Preview";

        public IList<Skillset> Skillsets
        {
            get
            {
                return GetSkillsets().GetAwaiter().GetResult();
            }
        }

        public IList<Index> Indexes
        {
            get
            {
                return GetIndexes().GetAwaiter().GetResult();
            }
        }

        public IList<Indexer> Indexers
        {
            get
            {
                return GetIndexers().GetAwaiter().GetResult();
            }
        }

        public async Task<Index> CreateIndex(Index index)
        {
            var uri = new Uri($"{client.BaseAddress}indexes/{index.Name}?api-version={ApiVersion}");
            var payload = await Task.Run(() => JsonConvert.SerializeObject(index));
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                var indexesResponse = await response.Content.ReadAsStringAsync();
                index = JsonConvert.DeserializeObject<Index>(indexesResponse);
            }

            return index;
        }

        public async Task<Indexer> CreateIndexer(Indexer indexer)
        {
            var uri = new Uri($"{client.BaseAddress}indexers/{indexer.Name}?api-version={ApiVersion}");
            var payload = await Task.Run(() => JsonConvert.SerializeObject(indexer));
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                var indexersResponse = await response.Content.ReadAsStringAsync();
                indexer = JsonConvert.DeserializeObject<Indexer>(indexersResponse);
            }

            return indexer;
        }

        public async Task<Skillset> CreateSkillset(Skillset skillset)
        {
            var uri = new Uri($"{client.BaseAddress}skillsets/{skillset.Name}?api-version={ApiVersion}");
            var payload = await Task.Run(() => JsonConvert.SerializeObject(skillset));
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                var skillsetsResponse = await response.Content.ReadAsStringAsync();
                skillset = JsonConvert.DeserializeObject<Skillset>(skillsetsResponse);
            }

            return skillset;
        }

        public async Task<bool> DeleteIndex(string indexName)
        {
            var uri = new Uri($"{client.BaseAddress}indexes/{indexName}?api-version={ApiVersion}");
            var response = await client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteIndexer(string indexerName)
        {
            var uri = new Uri($"{client.BaseAddress}indexers/{indexerName}?api-version={ApiVersion}");
            var response = await client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSkillset(string skillsetName)
        {
            var uri = new Uri($"{client.BaseAddress}skillsets/{skillsetName}?api-version={ApiVersion}");
            var response = await client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }

        public async Task<IList<Index>> GetIndexes()
        {
            List<Index> indexes = null;
            var uri = new Uri($"{client.BaseAddress}indexes?api-version={ApiVersion}");
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var indexesResponse = await response.Content.ReadAsStringAsync();
                var collectionResponse = JsonConvert.DeserializeObject<ApiCollectionResponse<Index>>(indexesResponse);
                indexes = collectionResponse.Value;
            }

            return indexes;
        }

        public async Task<IList<Indexer>> GetIndexers()
        {
            List<Indexer> indexers = null;
            var uri = new Uri($"{client.BaseAddress}indexers?api-version={ApiVersion}");
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var indexersResponse = await response.Content.ReadAsStringAsync();
                var collectionResponse = JsonConvert.DeserializeObject<ApiCollectionResponse<Indexer>>(indexersResponse);
                indexers = collectionResponse.Value;
            }

            return indexers;
        }

        public async Task<IList<Skillset>> GetSkillsets()
        {
            List<Skillset> skillsets = null;
            var uri = new Uri($"{client.BaseAddress}skillsets?api-version={ApiVersion}");
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var skillsetsResponse = await response.Content.ReadAsStringAsync();
                var collectionResponse = JsonConvert.DeserializeObject<ApiCollectionResponse<Skillset>>(skillsetsResponse);
                skillsets = collectionResponse.Value;
            }

            return skillsets;
        }

        //public async Task<Index> GetIndex(string indexName)
        //{
        //    Index index = null;
        //    var uri = new Uri($"{client.BaseAddress}indexes/{indexName}?api-version={ApiVersion}");
        //    var response = await client.GetAsync(uri);
            
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var indexResponse = await response.Content.ReadAsStringAsync();
        //        index = JsonConvert.DeserializeObject<Index>(indexResponse);
        //    }

        //    return index;
        //}

        //public Indexer GetIndexer(string indexerName)
        //{
        //    client.BaseAddress = new Uri($"{_searchServiceUrl}/indexers/{indexerName}?api-version={ApiVersion}");
        //}

        //public Skillset GetSkillset(string skillsetName)
        //{
        //    client.BaseAddress = new Uri($"{_searchServiceUrl}/skillsets/{skillsetName}?api-version={ApiVersion}");
        //}
    }
}