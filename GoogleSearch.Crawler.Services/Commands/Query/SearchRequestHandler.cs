using GoogleSearch.Crawler.Entities;
using GoogleSearch.Crawler.Services.Interfaces;
using GoogleSearch.Crawler.Services.Model;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleSearch.Crawler.Services.Commands.Query
{

    public class SearchRequestHandler : IRequestHandler<SearchRequest, SearchResult>
    {
        public const int MaxResult = 100;
        private readonly ISearchService searchService;
        private readonly ISearchParser searchParser;

        public SearchRequestHandler(ISearchService searchService, ISearchParser searchParser)
        {
            this.searchService = searchService;
            this.searchParser = searchParser;
        }

        public async Task<SearchResult> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            SearchPageResponse searchPageResponse;
            List<GoogleSearchResult> googleResults = new List<GoogleSearchResult>();
            int searchResultCount = 0;
            int page = 1;
            do
            {
                var searchContent = await searchService.SearchAsync(request);
                searchPageResponse = searchParser.TransformSearchResult(searchContent);
                int remaining = MaxResult - searchResultCount;
                googleResults.AddRange(searchParser.GetSearchResult(searchPageResponse.HtmlResults, remaining));
                searchResultCount += searchPageResponse.SearchItemCount;
                request.NextPage = $"{searchPageResponse.NextPage}&start={page * 10}";
                page += 1;
            } while (searchResultCount < MaxResult);

            return new SearchResult
            {
                KeywordsValue = searchParser.AggregateResult(request, googleResults),
                Url = request.Url
            };
        }

    }
}
