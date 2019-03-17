namespace Twitter.Common.Models
{
    public class TweetTextWithHashtags
    {
        public string Text { get; set; }
        public Hashtag[] Hashtags { get; set; }
    }
}