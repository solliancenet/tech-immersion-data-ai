using Newtonsoft.Json;

namespace Twitter.Common.Models
{
    /// <summary>
    /// The entities section will contain a hashtags array containing an object for every hashtag
    /// included in the Tweet body, and include an empty array if no hashtags are present.
    ///
    /// The PowerTrack # Operator is used to match on the text attribute.
    /// </summary>
    public class Hashtag
    {
        /// <summary>
        /// Array of integers indicating the offsets within the Tweet text whre the hashtag begins and ends.
        /// The first integer represents the location of the # character in the Tweet text string.
        /// The second integer represents the location of the first character after the hashtag.
        /// The distance between the to numbers will be the length of the hashtag name, plus one for the # character.
        /// </summary>
        [JsonProperty(PropertyName = "indices")]
        public int[] Indices { get; set; }

        /// <summary>
        /// Name of the hashtag, minus the leading '#' character.
        /// </summary>

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}