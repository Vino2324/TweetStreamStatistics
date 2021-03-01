using JHA.TweetStream.StatisticalServices.Interfaces.Statistics;
using JHA.TweetStream.StatisticalServices.Models.Statistics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{
    /// <summary>
    /// StatisticsService [Singleton] Class will be primary one to provide all tweet statistics to the caller. 
    /// </summary>
    public class StatisticsService : IStatisticsService
    {
        private readonly IEmojiService _emojiService;

        public StaticticalAverageService StaticticalAverages { get; private set; }

        public AverageAttributeStaticticsService EmojiStatictics { get; private set; }

        public AttributeStaticticsService HashtagStatictics { get; private set; }

        public UrlStaticticsService UrlStatictics { get; private set; }

        public StatisticsService(IEmojiService emojiService, StaticticalAverageService staticticalAverageService, AttributeStaticticsService hashtagStaticticsService, AverageAttributeStaticticsService emojiStaticticsService, UrlStaticticsService urlStaticticsService)
        {
            _emojiService = emojiService;
            EmojiStatictics = emojiStaticticsService;
            HashtagStatictics = hashtagStaticticsService;
            StaticticalAverages = staticticalAverageService;
            UrlStatictics = urlStaticticsService;
        }

        public void ProcessTweet(List<Tweet> tweets)
        {
            string str = string.Empty;
           
            
            Parallel.Invoke
            (
               () => { StaticticalAverages.Process(tweets.Count); },
               () =>
                {
                   var  tweetsWithHashtag = tweets.AsParallel()
                   .Where(t => t.Data?.HasHashTag == true)
                   .SelectMany(t => t.Data.Collection.HashTags.Select(ht => ht.Tag.ToLower()));
                    HashtagStatictics.AddToAttributeTracker(tweetsWithHashtag);
                },
               () =>
               {
                   foreach (Tweet tweet in tweets)
                   {
                       if (string.IsNullOrEmpty(tweet.Data?.Text)) continue;
                       var emojisInTweets = _emojiService.GetEmojisFromTweet(tweet.Data.Text);
                       EmojiStatictics.AddToAttributeTracker(emojisInTweets);
                       List<string> domains = new List<string>();
                       if (tweet.Data.HasUrl)
                       {
                           domains.AddRange(tweet.Data.Collection.Urls.Select(x =>
                           {
                               var result = x.Display_Url.Split('/');
                               return result[0];
                           }).ToList());

                           UrlStatictics.AddUrlProperties(domains);
                       }
                   }

                  
               }
            );
        }


        //public void ProcessTweet(List<Tweet> tweets)
        //{
        //    string str = string.Empty;
        //    List<string> domains = new List<string>();
        //    IEnumerable<string> tweetsWithHashtag = new List<string>();
        //    Parallel.Invoke
        //    (
        //       () => { StaticticalAverages.Process(tweets.Count); },
        //       () =>
        //        {
        //            foreach (Tweet tweet in tweets)
        //            {
        //                str = str + tweet.Data.Text;
        //                if (tweet.Data.HasUrl)
        //                {
        //                    domains.AddRange(tweet.Data.Collection.Urls.Select(x =>
        //                    {
        //                        var result = x.Display_Url.Split('/');
        //                        return result[0];
        //                    }).ToList());
        //                }
        //            }

        //            tweetsWithHashtag = tweets.AsParallel()
        //            .Where(t => t.Data.HasHashTag)
        //            .SelectMany(t => t.Data.Collection.HashTags.Select(ht => ht.Tag.ToLower()));
        //        }
        //    );

        //    Parallel.Invoke
        //        (
        //            () =>
        //            {
        //                var emojisInTweets = _emojiService.GetEmojisFromTweet(str);
        //                EmojiStatictics.AddToAttributeTracker(emojisInTweets, StaticticalAverages.TotalTweetCount); 
        //            },
        //            () =>
        //            {
        //                HashtagStatictics.AddToAttributeTracker(tweetsWithHashtag);
        //            },
        //            () =>
        //            {
        //                UrlStatictics.AddUrlProperties(domains, StaticticalAverages.TotalTweetCount);
        //            }
        //        );

        //}
    }
}
