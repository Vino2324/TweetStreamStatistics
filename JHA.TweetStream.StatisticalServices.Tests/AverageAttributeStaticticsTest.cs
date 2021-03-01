using JHA.TweetStream.StatisticalServices.Services.Statistics;
using NUnit.Framework;
using System.Collections.Generic;

namespace JHA.TweetStream.StatisticalServices.Tests
{
    public class AverageAttributeStaticticsTest
    {
        private AverageAttributeStaticticsService _service;
        [SetUp]
        public void Setup()
        {
            _service = new AverageAttributeStaticticsService();
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 0)]        
        [TestCase(1, 2, 33.33)]
        [TestCase(2, 2, 50)]
        public void AverageAttributeStatictics_Cases(int attributesCount, int nonAttributesCount, double result)
        {
            // arrange
            var propertyList = new List<string>() { "Heart", "fuelpump" };

            // act
            for (int i = 0; i < attributesCount; i++)
            {
                _service.AddToAttributeTracker(propertyList);
            }
            for (int i = 0; i < nonAttributesCount; i++)
            {
                _service.AddToAttributeTracker(new string[] { });
            }

            // assert
            Assert.AreEqual(result, _service.AttributePercentage);
        }
    }
}
