using Newtonsoft.Json;

namespace DataGenerator.Models
{
    public class Symbol
    {
        /// <summary>
        /// An array of integers representing the offsets within the Tweet text where the symbol/cashtag
        /// begins and ends. 
        /// The first integer represents the location of the ‘$’ character of the user mention. 
        /// The second integer represents the location of the first non-screenname character following the
        /// cashtag.
        /// </summary>
        [JsonProperty(PropertyName = "indices")]
        public int[] indices { get; set; }

        /// <summary>
        /// Name of the hashtag, minus the leading '$' character.
        /// </summary>

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}