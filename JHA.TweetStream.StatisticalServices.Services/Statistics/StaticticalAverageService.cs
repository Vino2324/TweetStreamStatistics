using System;

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{
    /// <summary>
    /// Class to calculate Averages for the tweet received from sample stream.
    /// </summary>
    public class StaticticalAverageService
    {
        private DateTime _startTime = DateTime.MinValue;
        public int TotalTweetCount { get; private set; } = 0;
        public double TweetPerHour { get; private set; } = 0;
        public double TweetPerMinute { get; private set; } = 0;
        public double TweetPerSecond { get; private set; } = 0;

        public void Process(int count)
        {
            TotalTweetCount = TotalTweetCount + count;
            
            if (_startTime == DateTime.MinValue) { _startTime = DateTime.UtcNow; }

            var timeSpan = DateTime.UtcNow - _startTime;
            TweetPerSecond = timeSpan.TotalSeconds > 0 ? TotalTweetCount / timeSpan.TotalSeconds : TotalTweetCount;
            TweetPerMinute = timeSpan.TotalMinutes > 0 ? TotalTweetCount / timeSpan.TotalMinutes : TotalTweetCount;
            TweetPerHour = timeSpan.TotalHours > 0 ? TotalTweetCount / timeSpan.TotalHours : TotalTweetCount;
        }

    }
}
