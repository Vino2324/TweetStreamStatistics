using System.Collections.Generic;

namespace JHA.TweetStream.StatisticalServices.Interfaces.Statistics
{
    public interface IEmojiService
    {
        List<string> GetEmojisFromTweet(string text);
    }
}
