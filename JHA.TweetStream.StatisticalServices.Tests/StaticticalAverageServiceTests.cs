using JHA.TweetStream.StatisticalServices.Services.Statistics;
using NUnit.Framework;
using System;

namespace JHA.TweetStream.StatisticalServices.Tests
{
    public class StaticticalAverageServiceTests
    {
        private StaticticalAverageService _staticticalAverageService;
        [SetUp]
        public void Setup()
        {
            _staticticalAverageService = new StaticticalAverageService();
        }

        [TestCase(230)]
        [TestCase(11)]
        [TestCase(3)]
        public void StaticticalAverage_Compute(int tweetsReceived)
        {
  
            _staticticalAverageService.Process(tweetsReceived);


            var elapsedSeconds = _staticticalAverageService.TotalTweetCount / _staticticalAverageService.TweetPerSecond; 

            var tweetsPerSecond = tweetsReceived / elapsedSeconds;
            var tweetsPerMinute = Math.Round(_staticticalAverageService.TweetPerSecond * 60, 5);
            var tweetsPerHour = Math.Round(_staticticalAverageService.TweetPerMinute * 60, 5);

            var actualtweetsPerSecond = _staticticalAverageService.TweetPerSecond;
            var actualtweetsPerMinute = Math.Round(_staticticalAverageService.TweetPerMinute, 5);
            var actualtweetsPerHour = Math.Round(_staticticalAverageService.TweetPerHour, 5);


            Assert.AreEqual(tweetsReceived, _staticticalAverageService.TotalTweetCount);
            Assert.AreEqual(tweetsPerSecond, actualtweetsPerSecond);
            Assert.AreEqual(tweetsPerMinute, actualtweetsPerMinute);
            Assert.AreEqual(tweetsPerHour, actualtweetsPerHour);
        }
    }
}