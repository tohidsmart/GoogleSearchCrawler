using GoogleSearch.Crawler.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSearch.Crawler.Services.Model
{
    public class SearchPageResponse
    {
        public string NextPage { get; set; }
        public int SearchItemCount { get; set; }
        public HtmlNodeCollection HtmlResults { get; set; }
        public List<GoogleSearchResult> GoogleSearchResult { get; set; }
    }
}
