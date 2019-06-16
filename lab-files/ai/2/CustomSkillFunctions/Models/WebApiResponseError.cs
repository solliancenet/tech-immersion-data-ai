using Newtonsoft.Json;

namespace CustomSkillFunctions.Models
{
    public class WebApiResponseError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}