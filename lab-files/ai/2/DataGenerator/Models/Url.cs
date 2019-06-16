using Newtonsoft.Json;

namespace DataGenerator.Models
{
    public class Url
    {
        [JsonProperty(PropertyName = "url")]
        public string UrlStr { get; set; }
    }
}