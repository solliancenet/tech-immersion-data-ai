using Newtonsoft.Json;
using System.Collections.Generic;

namespace CustomSkillFunctions.Models
{
    public class WebApiResponseRecord
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
        [JsonProperty("errors")]
        public List<WebApiResponseError> Errors { get; set; }
        [JsonProperty("warnings")]
        public List<WebApiResponseWarning> Warnings { get; set; }
    }
}