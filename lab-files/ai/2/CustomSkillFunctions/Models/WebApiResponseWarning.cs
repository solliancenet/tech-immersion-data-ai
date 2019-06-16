using Newtonsoft.Json;

namespace CustomSkillFunctions.Models
{
    public class WebApiResponseWarning
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}