using Newtonsoft.Json;

namespace PipelineEnhancer.Models
{
    public class FormRecognizerTrainRequestBody
    {
        [JsonProperty("source")]
        public string Source { get; set; }
    }
}