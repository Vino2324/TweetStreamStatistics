using JHA.TweetStream.StatisticalServices.Services.Statistics;
using NUnit.Framework;

namespace JHA.TweetStream.StatisticalServices.Tests
{
    public class EmojiServiceTest
    {
        private EmojiService _emojiService;
        [SetUp]
        public void Setup()
        {
            _emojiService = new EmojiService();
        }

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("Emoji Unicode Reference.", 0)]
        [TestCase("Fill the ⛽ today", 1)]
        [TestCase("Price of Costco ⛽, Chevron ⛽, Shell ⛽ are more today", 1)]
        [TestCase("Fill the ⛽ today or eat a 🍩 but never drink many 🍻", 3)]
        public void EmojiService_GetEmojisFromTweet(string tweet, int expectedEmojiCount)
        {
            var results = _emojiService.GetEmojisFromTweet(tweet);

            // assert
            Assert.AreEqual(expectedEmojiCount, results.Count);
        }
    }
}
