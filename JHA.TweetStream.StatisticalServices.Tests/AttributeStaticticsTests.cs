using JHA.TweetStream.StatisticalServices.Services.Statistics;
using NUnit.Framework;
using System.Linq;

namespace JHA.TweetStream.StatisticalServices.Tests
{
    public class AttributeStaticticsTests
    {

        private AttributeStaticticsService _service;
        [SetUp]
        public void Setup()
        {
            _service = new AttributeStaticticsService();
        }

        [TestCase(null, 0, 0)]
        [TestCase(new string[] { "COVID19" }, 1, 1)]
        [TestCase(new string[] { "COVID19", "COVID19" }, 1, 2)]
        public void HashtagService_GetHashtagFromTweet(string[] propertyList, int expectedLength, int expectedCount)
        {

                _service.AddToAttributeTracker(propertyList);


            Assert.AreEqual(expectedLength, _service.TopAttributes.Count());


            if (expectedLength > 0)
            {
                var expectedString = $"Hashtag:COVID19 Count:{expectedCount}";
                Assert.AreEqual(expectedString, _service.TopAttributes.First());
            }
        }
    }


}
