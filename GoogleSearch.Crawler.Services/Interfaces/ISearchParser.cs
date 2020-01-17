using GoogleSearch.Crawler.Entities;
using GoogleSearch.Crawler.Services.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSearch.Crawler.Services.Interfaces
{
    public interface ISearchParser
    {
        SearchPageResponse TransformSearchResult(string rawHtml);
        Dictionary<string, int> AggregateResult(SearchRequest request, List<GoogleSearchResult> googleResult);
         List<GoogleSearchResult> GetSearchResult(HtmlNodeCollection htmlNodes,int count);
    }
}
