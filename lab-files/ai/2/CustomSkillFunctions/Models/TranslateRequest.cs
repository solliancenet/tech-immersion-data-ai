using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CustomSkillFunctions.Models
{
    public class TranslateRequest
    {
        [JsonProperty("values")]
        public List<TranslateValue> Values { get; set; }
    }

    public class TranslateValue
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }
        [JsonProperty("data")]
        public TranslateData Data {get;set;}
    }

    public class TranslateData
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
    }
}