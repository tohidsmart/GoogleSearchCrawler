using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSearch.Crawler.Entities
{
    public class SearchResult
    {
        public Dictionary<string, int> KeywordsValue { get; set; }
        public string Url { get; set; }
    }
}
