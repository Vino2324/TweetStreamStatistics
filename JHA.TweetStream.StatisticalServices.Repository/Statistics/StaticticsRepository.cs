using JHA.TweetStream.StatisticalServices.Interfaces.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JHA.TweetStream.StatisticalServices.Repository.Statistics
{
    public class HashtagStaticticsRepository : IStatisticsRepository
    {
        private const int DISPLAY_TOP_COUNT = 10;

        protected string _propertyName = "Hashtag";
        private Dictionary<string, int> _allProperties = new Dictionary<string, int>();

        public IEnumerable<string> TopProperties { get { return _allProperties.OrderByDescending(x => x.Value).Take(DISPLAY_TOP_COUNT).Select(x => $"{_propertyName}:{x.Key} Count:{x.Value}"); } }

        public HashtagStaticticsRepository()
        {

        }

        public void AddProperties(IEnumerable<string> propertyNames)
        {
            if (propertyNames == null)
                return;

            foreach (var name in propertyNames)
            {
                if (_allProperties.ContainsKey(name))
                {
                    _allProperties[name]++;
                }
                else
                {
                    _allProperties.Add(name, 1);
                }
            }
        }
    }

    public class EmojiStaticticsRepository : HashtagStaticticsRepository
    {
        private int _tweetsWithProperty = 0;
        protected int _totalTweets = 0;

        public double PercentWithProperty { get { return _totalTweets != 0 ? ((double)_tweetsWithProperty / (double)_totalTweets) * 100 : 0; } }

        public EmojiStaticticsRepository()
        {
            _propertyName = "Emoji";
        }

        public new void AddProperties(IEnumerable<string> propertyNames)
        {
            _totalTweets++;

            if (propertyNames.Any())
            {
                _tweetsWithProperty++;
            }

            base.AddProperties(propertyNames);
        }
    }

    public class StaticticalAverageRepository
    {
        private DateTime _startTime = DateTime.MinValue;
        public int TotalTweetCount { get; private set; } = 0;
        public double TweetPerHour { get; private set; } = 0;
        public double TweetPerMinute { get; private set; } = 0;
        public double TweetPerSecond { get; private set; } = 0;
        public void Recompute()
        {
            if (_startTime == DateTime.MinValue)
                _startTime = DateTime.Now;

            var currentTime = DateTime.Now;

            TotalTweetCount++;

            // compute averages
            // timespan should never be exactly 0 (at least a millisecond or two), but protect against divide by zero just in case
            var timeSpan = currentTime - _startTime;
            TweetPerSecond = timeSpan.TotalSeconds > 0 ? TotalTweetCount / timeSpan.TotalSeconds : TotalTweetCount;
            TweetPerMinute = timeSpan.TotalMinutes > 0 ? TotalTweetCount / timeSpan.TotalMinutes : TotalTweetCount;
            TweetPerHour = timeSpan.TotalHours > 0 ? TotalTweetCount / timeSpan.TotalHours : TotalTweetCount;
        }

    }
}
