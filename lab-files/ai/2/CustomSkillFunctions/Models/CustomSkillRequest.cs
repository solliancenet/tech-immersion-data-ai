using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomSkillFunctions.Models
{
    public class CustomSkillRequest
    {
        [JsonProperty("values")]
        public List<CustomSkillValue> Values { get; set; }
    }

    public class CustomSkillValue
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }
        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}
