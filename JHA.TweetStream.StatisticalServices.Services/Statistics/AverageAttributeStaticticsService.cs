using System.Collections.Generic;
using System.Linq;

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{
    public class AverageAttributeStaticticsService : AttributeStaticticsService
    {
        private int _attributeTweets = 0;
        protected  int _totalTweetCount = 0;
        public double AttributePercentage 
        {
            get 
            {
                return _totalTweetCount != 0 ? ((double)_attributeTweets / (double)_totalTweetCount) * 100 : 0; 
            }
        }

        public AverageAttributeStaticticsService()  { _attributeName = "Emoji"; }

        //public void AddToAttributeTracker(IEnumerable<string> attributeNames, int count)
        //{
        //    attributeNames = attributeNames ?? new List<string>();
        //    _totalTweetCount =  count;
           
        //    if (attributeNames.Any()) { _attributeTweets++; } //_attributeTweets = _attributeTweets + attributeNames.Distinct().Count();
        //    base.AddToAttributeTracker(attributeNames);
        //}
        public new void AddToAttributeTracker(IEnumerable<string> attributeNames)
        {
            attributeNames = attributeNames ?? new List<string>();
            //_totalTweetCount =  count;
            _totalTweetCount++;
            if (attributeNames.Any()) { _attributeTweets++; } //_attributeTweets = _attributeTweets + attributeNames.Distinct().Count();
            base.AddToAttributeTracker(attributeNames);
        }
    }
}
