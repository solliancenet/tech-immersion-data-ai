using Newtonsoft.Json;

namespace Twitter.Common.Models
{
    public class Url
    {
        [JsonProperty(PropertyName = "url")]
        public string UrlStr { get; set; }
    }
}