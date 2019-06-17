using Newtonsoft.Json;

namespace DataGenerator.Models
{
    public class Vehicle
    {
        [JsonProperty(PropertyName = "make")]
        public string Make { get; set; }
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }
    }
}