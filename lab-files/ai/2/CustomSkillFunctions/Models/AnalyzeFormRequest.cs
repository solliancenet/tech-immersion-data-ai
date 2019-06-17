using Newtonsoft.Json;
using System.Collections.Generic;

namespace CustomSkillFunctions.Models
{
    public class AnalyzeFormRequest
    {
        [JsonProperty("values")]
        public List<AnalyzeFormValue> Values { get; set; }
    }

    public class AnalyzeFormValue
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }
        [JsonProperty("data")]
        public AnalyzeFormData Data { get; set; }
    }

    public class AnalyzeFormData
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
        [JsonProperty("storageUri")]
        public string StorageUri { get; set; }
        [JsonProperty("storageSasToken")]
        public string StorageSasToken { get; set; }
    }
}