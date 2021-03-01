using System.Collections.Generic;

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{
    public class UrlStaticticsService: AverageAttributeStaticticsService
    {
        private List<string> _photoDomains = new List<string>() { "pic.twitter.com", "instagram.com" };
        private int photoUrlTweetCount = 0;       

        public double PhotoUrlPercentage 
        { 
            get 
            { 
                return _totalTweetCount != 0 ? ((double)photoUrlTweetCount / (double)_totalTweetCount) * 100 : 0; 
            } 
        }

        public UrlStaticticsService()  { _attributeName = "Domain"; }

        //public void AddUrlProperties(List<string> domains,int count)
        //{
        //    totalTweetCount = count;
        //    foreach (var item in domains ?? new List<string>())
        //    {
        //        if (_photoDomains.Contains(item))  { photoUrlTweetCount++;  }
        //    }

        //    base.AddToAttributeTracker(domains, count);
        //}

        public void AddUrlProperties(List<string> domains)
        {            
            foreach (var item in domains ?? new List<string>())
            {
                if (_photoDomains.Contains(item)) { photoUrlTweetCount++; }
            }

            base.AddToAttributeTracker(domains);
        }

    }
}
