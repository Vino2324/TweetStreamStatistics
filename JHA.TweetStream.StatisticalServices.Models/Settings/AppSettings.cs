namespace JHA.TweetStream.StatisticalServices.Models.Settings
{
    public class AppSettings
    {
        public string AuthorizeToken { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string StreamTweetUrl { get; set; }

        public int RetryLimit { get; set; } = -1;
    }
}
