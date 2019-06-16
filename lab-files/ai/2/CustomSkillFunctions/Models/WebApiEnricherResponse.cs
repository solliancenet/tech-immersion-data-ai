using Newtonsoft.Json;
using System.Collections.Generic;

namespace CustomSkillFunctions.Models
{
    public class WebApiEnricherResponse
    {
        [JsonProperty("values")]
        public List<WebApiResponseRecord> Values { get; set; }
    }
}