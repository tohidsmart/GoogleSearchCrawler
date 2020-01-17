using GoogleSearch.Crawler.Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSearch.Crawler.Services.Interfaces
{
    public interface ISearchService
    {
        Task<string> SearchAsync(SearchRequest request);
    }
}
