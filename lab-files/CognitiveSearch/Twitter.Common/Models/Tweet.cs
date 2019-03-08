using Newtonsoft.Json;
using System;

namespace Twitter.Common.Models
{
    public class Tweet
    {
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        //[JsonProperty(PropertyName = "id")]
        //public long Id
        //{
            
        //}

        [JsonProperty(PropertyName = "id_str")]
        public string IdStr
        {
            get
            {
                var r = new Random();
                return r.Next(100000000, 999999999).ToString();
            }
        }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        [JsonProperty(PropertyName = "entities")]
        public Entity Entities { get; set; }
    }
}