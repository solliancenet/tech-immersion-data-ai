using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataGenerator.Models
{
    public class User
    {
        public User()
        {
            Location = GetLocation();
        }
        
        [JsonProperty(PropertyName = "id")]
        public long Id
        {
            get
            {
                var r = new Random();
                return r.Next(10000000, 999999999);
            }
        }

        [JsonProperty(PropertyName = "id_str")]
        public string IdStr
        {
            get { return Id.ToString(); }
        }

        // Capped at 20 chars
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName
        {
            get { return Name.Replace(" ", ""); }
        }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; } = "";

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "";

        private static string GetLocation()
        {
            var list = new List<string>
            {
                "Mobile, AL",
                "Phoenix, AZ",
                "Tucson, AZ",
                "San Diego, CA",
                "San Francisco, CA",
                "San Jose, CA",
                "Los Angeles, CA",
                "Sacramento, CA",
                "Redding, CA",
                "Anaheim, CA",
                "Long Beach, CA",
                "Fresno, CA",
                "Denver, CO",
                "Dover, DE",
                "Miami, FL",
                "Orlando, FL",
                "Tampa, FL",
                "Chicago, IL",
                "Indianapolis, IN",
                "Kansas City, KS",
                "Boston, MA",
                "Detroit, MI",
                "St. Louis, MO",
                "Las Vegas, NV",
                "New York, NY",
                "Portland, OR",
                "Philadelphio, PA",
                "Dallas, TX",
                "Austin, TX",
                "Houston, TX",
                "Virginia Beach, VA",
                "Seattle, WA",
                "Madison, WI",
                "Jackon, WY" };

            var count = list.Count;
            var r = new Random();
            var num = r.Next(count);
            return list[num];
        }
    }
}