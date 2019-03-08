using Newtonsoft.Json;

namespace Twitter.Common.Models
{
    /// <summary>
    /// The entities section will contain a user_mentions array containing an object for every user
    /// mention included in the Tweet body, and include an empty array if no user mention is present.
    /// 
    /// The PowerTrack @ Operator is used to match on the screen_name attribute.
    /// </summary>
    public class UserMention
    {
        /// <summary>
        /// ID of the mentioned user, as an integer. 
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        /// <summary>
        /// If of the mentioned user, as a string.
        /// </summary>
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr
        {
            get { return Id.ToString(); }
        }

        /// <summary>
        /// An array of integers representing the offsets within the Tweet text where the user reference
        /// begins and ends. 
        /// The first integer represents the location of the ‘@’ character of the user mention. 
        /// The second integer represents the location of the first non-screenname character following the
        /// user mention.
        /// </summary>
        [JsonProperty(PropertyName = "indices")]
        public int[] Indices { get; set; }

        /// <summary>
        /// Display name of the referenced user.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Screen name of the referenced user.
        /// </summary>
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
    }
}