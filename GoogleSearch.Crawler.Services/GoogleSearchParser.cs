using GoogleSearch.Crawler.Entities;
using GoogleSearch.Crawler.Services.Interfaces;
using GoogleSearch.Crawler.Services.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleSearch.Crawler.Services
{
    public class GoogleSearchParser : ISearchParser
    {
        public Dictionary<string, int> AggregateResult(SearchRequest request, List<GoogleSearchResult> googleResult)
        {

            Dictionary<string, int> aggregatedResult = request.KeyWordsList.ToDictionary(k => k, v => 0);
            foreach (var searchResult in googleResult)
            {
                var keywords = request.KeyWordsList.Where(keyword => IsKeywordOccured(keyword, searchResult.CitationText,
                                                                      searchResult.CitationUrl, request.Url));
                foreach (var keyword in keywords)
                {
                    if (aggregatedResult.ContainsKey(keyword))
                    {
                        aggregatedResult[keyword] = aggregatedResult[keyword] + 1;
                    }
                    else
                    {
                        aggregatedResult[keyword] = 1;
                    }
                }
            };
            return aggregatedResult;
        }

        public SearchPageResponse TransformSearchResult(string rawHtml)
        {
            string allResultXPath = ".//div[@id='main']/div";
            var searchResponse = new SearchPageResponse();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(rawHtml);

            var nextPageValue = htmlDocument.DocumentNode.SelectSingleNode(".//footer//a").Attributes["href"]?.Value;
            var nextPageQuery = nextPageValue?.Split("?", StringSplitOptions.RemoveEmptyEntries).Last();
            searchResponse.NextPage = nextPageQuery;
            searchResponse.HtmlResults = htmlDocument.DocumentNode.SelectNodes(allResultXPath);
            searchResponse.HtmlResults?.Remove(searchResponse.HtmlResults?.First());
            searchResponse.HtmlResults?.Remove(searchResponse.HtmlResults?.Last());
            searchResponse.SearchItemCount = searchResponse.HtmlResults == null ? 0 : searchResponse.HtmlResults.Count;
            return searchResponse;
        }

        public List<GoogleSearchResult> GetSearchResult(HtmlNodeCollection htmlNodes, int count)
        {
            List<GoogleSearchResult> googleSearchResults = new List<GoogleSearchResult>();
            var nodesToProcess = htmlNodes.Count < count ? htmlNodes : htmlNodes.Take(count);

            foreach (var htmlNode in nodesToProcess)
            {
                GoogleSearchResult googleSearchResult = new GoogleSearchResult
                {
                    CitationUrl = htmlNode?.SelectSingleNode(".//a")?.InnerText,
                    CitationText = htmlNode?.InnerText
                };
                googleSearchResults.Add(googleSearchResult);
            }
            return googleSearchResults;
        }

        private bool IsKeywordOccured(string keyword, string text, string textUrl, string requestUrl)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(textUrl))
                return false;
            bool occured = text.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                          && textUrl.Contains(requestUrl, StringComparison.OrdinalIgnoreCase);
            return occured;
        }
    }
}
