using Newtonsoft.Json;
using System.Collections.Generic;

namespace JHA.TweetStream.StatisticalServices.Models.Statistics
{
    public class Tweet
    {
        public TweetObject Data { get; set; }
    }

    public class TweetObject
    {

        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("entities")]
        public TweetCollection Collection { get; set; }

        public bool HasHashTag { get { return null != Collection && null != Collection.HashTags && 0 < Collection.HashTags.Count; } }
        public bool HasUrl { get { return null != Collection && null != Collection.Urls && 0 < Collection.Urls.Count; } }      
    }

    public class TweetCollection
    {
        [JsonProperty("hashtags")]
        public List<TweetHashTag> HashTags { get; set; }
        [JsonProperty("urls")]
        public List<TweetUrl> Urls { get; set; }

    }

    public class TweetHashTag
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }


    public class TweetUrl
    {
        [JsonProperty("url")]
        public string TwitterUrl { get; set; }
        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }
        [JsonProperty("display_url")]
        public string Display_Url { get; set; }
    }

}
