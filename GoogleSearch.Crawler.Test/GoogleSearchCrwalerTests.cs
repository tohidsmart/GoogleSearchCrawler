using GoogleSearch.Crawler.Services;
using GoogleSearch.Crawler.Services.Interfaces;
using GoogleSearch.Crawler.Services.Model;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleSearch.Crawler.Test
{
    [TestClass]
    public class GoogleSearchCrawlerTests
    {
        private readonly ISearchService SearchService;
        private readonly ISearchParser SearchParser;
        private HtmlDocument HtmlDocument;
        private string RawHtml;
        private SearchPageResponse SearchPageResponse;
        public const int MaxResult = 8;
        static SearchRequest SearchRequest;


        public GoogleSearchCrawlerTests()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.google.com")
            };
            SearchService = new GoogleSearchService(httpClient);
            SearchParser = new GoogleSearchParser();
            SearchPageResponse = new SearchPageResponse();
            SearchRequest = new SearchRequest
            {
                Keywords = "Infotrack webvoi",
                Url = "infotrack.com.au"
            };
        }

        /// <summary>
        /// Initialize each property for indiviual unit test
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async Task TestInitilize()
        {
            RawHtml = await SearchService.SearchAsync(SearchRequest);
            HtmlDocument = new HtmlDocument();
            HtmlDocument.LoadHtml(RawHtml);
            SearchPageResponse = SearchParser.TransformSearchResult(RawHtml);

        }

        /// <summary>
        /// Given the document is download
        /// Check if it is a valid html document
        /// </summary>
        [TestMethod]
        public void ScrapResultTest()
        {
            var head = HtmlDocument.DocumentNode.SelectSingleNode("html/head");
            var body = HtmlDocument.DocumentNode.SelectSingleNode("html//body");

            Assert.IsTrue(head != null && body != null);

        }


        /// <summary>
        /// Given the html document is parsed
        /// Check if htmlcollection contains result 
        /// </summary>
        [TestMethod]
        public void TransformResultTest()
        {
            Assert.IsNotNull(SearchPageResponse.HtmlResults);
            Assert.IsNotNull(SearchPageResponse.NextPage);
            Assert.AreNotEqual(SearchPageResponse.SearchItemCount, 0);
        }

        /// <summary>
        /// Given that document is parsed and transfromed 
        /// Construct the custom entity from the search result 
        /// Aggregate the result and check for keyword in the aggregated result
        /// </summary>
        [TestMethod]
        public void AggregateResultTest()
        {
            int searchResultCount = 0;
            int remaining = MaxResult - searchResultCount;
            var searchResult = SearchParser.GetSearchResult(SearchPageResponse.HtmlResults, remaining);
            var aggregateResult = SearchParser.AggregateResult(SearchRequest, searchResult);

            Assert.IsNotNull(searchResult);
            Assert.AreEqual(MaxResult, searchResult.Count);
            foreach (var keywords in SearchRequest.KeyWordsList)
            {
                Assert.IsTrue(aggregateResult.ContainsKey(keywords));
                Assert.AreNotEqual(aggregateResult["webvoi"], 0);
            }
        }
    }
}
