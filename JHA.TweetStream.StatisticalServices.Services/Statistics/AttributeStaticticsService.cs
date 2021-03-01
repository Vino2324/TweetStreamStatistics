using System.Collections.Generic;
using System.Linq;

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{
    public class AttributeStaticticsService
    {
        private const int DISPLAY_COUNT = 5;

        protected string _attributeName = "Hashtag";
        private Dictionary<string, int> attributeList = new Dictionary<string, int>();

        public IEnumerable<string> TopAttributes
        {
            get
            {
                return attributeList.OrderByDescending(x => x.Value).Take(DISPLAY_COUNT).Select(x => $"{_attributeName}:{x.Key} Count:{x.Value}");
            }
        }

        public void AddToAttributeTracker(IEnumerable<string> attributeNames)
        {
            if (attributeNames == null) { return; }

            foreach (var name in attributeNames)
            {
                if (attributeList.ContainsKey(name))
                {
                    attributeList[name]++;
                }
                else
                {
                    attributeList.Add(name, 1);
                }
            }
        }
    }
}
