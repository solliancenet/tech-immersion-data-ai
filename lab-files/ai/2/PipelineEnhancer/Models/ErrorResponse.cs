using Newtonsoft.Json;

namespace PipelineEnhancer.Models
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}