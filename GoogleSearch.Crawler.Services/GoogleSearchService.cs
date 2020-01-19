using GoogleSearch.Crawler.Services.Interfaces;
using GoogleSearch.Crawler.Services.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleSearch.Crawler.Services
{
    public class GoogleSearchService : ISearchService
    {
        private readonly HttpClient httpClient;

        public GoogleSearchService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// This method scraps the content from Google based on Search Request parameter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> SearchAsync(SearchRequest request)
        {
            if (string.IsNullOrEmpty(request.Keywords))
            {
                throw new InvalidOperationException("One or more keyword missing");
            }
            var searchPrefix = "/search?";
            var searchPostFix = string.IsNullOrEmpty(request.NextPage) ? $"q={string.Join("+", request.KeyWordsList)}" : request.NextPage;
            var seachquery = $"{searchPrefix}{searchPostFix}";
            string responseBody = await httpClient.GetStringAsync(seachquery);
            return responseBody;
        }
    }
}
