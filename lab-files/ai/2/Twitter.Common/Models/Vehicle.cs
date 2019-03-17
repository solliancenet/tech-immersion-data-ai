using Newtonsoft.Json;

namespace Twitter.Common.Models
{
    public class Vehicle
    {
        [JsonProperty(PropertyName = "make")]
        public string Make { get; set; }
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }
    }
}