using Newtonsoft.Json;

namespace CustomSkillFunctions.Models
{
    public class AnomalyDetectionResponse
    {
        [JsonProperty("isAnomaly")]
        public bool IsAnomaly { get; set; }
        [JsonProperty("isPositiveAnomaly")]
        public bool IsPositiveAnomaly { get; set; }
        [JsonProperty("isNegativeAnomaly")]
        public bool IsNegativeAnomaly { get; set; }
        [JsonProperty("period")]
        public int Period { get; set; }
        [JsonProperty("expectedValue")]
        public float ExpectedValue { get; set; }
        [JsonProperty("upperMargin")]
        public float UpperMargin { get; set; }
        [JsonProperty("lowerMargin")]
        public float LowerMargin { get; set; }
        [JsonProperty("suggestedWindow")]
        public int SuggestedWindow { get; set; }
    }
}