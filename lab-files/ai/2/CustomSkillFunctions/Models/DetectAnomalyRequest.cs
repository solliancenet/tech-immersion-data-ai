using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CustomSkillFunctions.Models
{
    public class DetectAnomalyRequest
    {
        [JsonProperty("values")]
        public List<DetectAnomalyValue> Values { get; set; }
    }

    public class DetectAnomalyValue
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }
        [JsonProperty("data")]
        public DetectAnomalyData Data { get; set; }
    }

    public class DetectAnomalyData
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("engineTemperature")]
        public float EngineTemperature { get; set; }
    }
}