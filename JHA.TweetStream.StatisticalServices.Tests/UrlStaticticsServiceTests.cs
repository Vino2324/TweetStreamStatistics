using JHA.TweetStream.StatisticalServices.Services.Statistics;
using NUnit.Framework;
using System.Collections.Generic;

namespace JHA.TweetStream.StatisticalServices.Tests
{
    public class UrlStaticticsServiceTests
    {
        private UrlStaticticsService _service;

        [SetUp]
        public void Setup()
        {
            _service = new UrlStaticticsService();
        }


        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 0, 0, 100)] 
        [TestCase(1, 4, 0, 20)]
        [TestCase(1, 2, 5, 12.5)]
        public void UrlService_GetUrlsFromTweet(int picDomainCount, int nonPicDomainCount, int nonUrlCount, double result)
        {

            var urlListWithPicDomain = new List<string>() { new string ("pic.twitter.com") };
            var urlListWithoutPicDomain = new List<string>() { new string("google.com") };

            // act
            for (int i = 0; i < picDomainCount; i++)
            {
                _service.AddUrlProperties(urlListWithPicDomain);
            }

            for (int i = 0; i < nonPicDomainCount; i++)
            {
                _service.AddUrlProperties(urlListWithoutPicDomain);
            }

            for (int i = 0; i < nonUrlCount; i++)
            {
                _service.AddUrlProperties(new List<string>());
            }

            // assert
            Assert.AreEqual(result, _service.PhotoUrlPercentage);
        }
    }


}
