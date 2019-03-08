using Newtonsoft.Json;

namespace Twitter.Common.Models
{
    public class Entity
    {
        /// <summary>
        /// Represents hashtags which have been parsed out of the Tweet text.
        /// Array of Hashtag objects.
        /// </summary>
        [JsonProperty(PropertyName = "hashtags")]
        public Hashtag[] Hashtags { get; set; }

        /// <summary>
        /// Represents other Twitter users mentioned in the text of the Tweet.
        /// Array of UserMention objects.
        /// </summary>
        [JsonProperty(PropertyName = "user_mentions")]
        public UserMention[] UserMentions { get; set; }

        // These will be empty arrays for this demo...

        /// <summary>
        /// Represents symbols, i.e. $cashtags, included in the text of the Tweet.
        /// </summary>
        [JsonProperty(PropertyName = "symbols")]
        public Symbol[] Symbols { get; set; }

        /// <summary>
        /// Represents URLs included in the text of a Tweet.
        /// </summary>
        [JsonProperty(PropertyName = "urls")]
        public Url[] Urls { get; set; }
    }
}