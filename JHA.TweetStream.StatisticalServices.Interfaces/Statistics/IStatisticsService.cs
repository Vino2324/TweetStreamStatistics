using JHA.TweetStream.StatisticalServices.Models.Statistics;
using System.Collections.Generic;

namespace JHA.TweetStream.StatisticalServices.Interfaces.Statistics
{
    public interface IStatisticsService
    {
        void ProcessTweet(List<Tweet> tweets);
    }
}
