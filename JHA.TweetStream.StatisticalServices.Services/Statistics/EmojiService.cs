using JHA.TweetStream.StatisticalServices.Interfaces.Statistics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{

    public class EmojiService : IEmojiService
    {
        private const string JSON_FILE = "Resources/emoji.json";

        
        private readonly Dictionary<string, string> _emojis;

        /// <summary>
        /// we are interested in 2 properties: "short_name" and "unified"[unicode representation] property.
        /// </summary>
        public EmojiService()
        {
            // load json 
            var data = JArray.Parse(File.ReadAllText(JSON_FILE));
            _emojis = data.OfType<JObject>().ToDictionary(
                // key dictionary by short names             
                c =>  ((JValue)c["short_name"]).Value.ToString() ,
                c => {
                    var unicodeRaw = ((JValue)c["unified"]).Value.ToString();
                    var chars = new List<char>();
                // some characters are multibyte in UTF32, split them
                foreach (var point in unicodeRaw.Split('-'))
                    {
                    // parse hex to 32-bit unsigned integer (UTF32)
                    uint unicodeInt = uint.Parse(point, System.Globalization.NumberStyles.HexNumber);
                    // convert to bytes and get chars with UTF32 encoding
                    chars.AddRange(Encoding.UTF32.GetChars(BitConverter.GetBytes(unicodeInt)));
                    }
                // this is resulting emoji
                return new string(chars.ToArray());
                });
        }

        public List<string> GetEmojisFromTweet(string tweet)
        {
            if (string.IsNullOrEmpty(tweet)) { return new List<string>(); }
            return _emojis.Where(x=> tweet.Contains(x.Value)).Select(x => x.Key).ToList();
        }
    }
}
