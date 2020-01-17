using GoogleSearch.Crawler.Entities;
using GoogleSearch.Crawler.Services.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GoogleSearch.Crawler.Services.Model
{
    public class SearchRequest : IRequest<SearchResult>
    {
        private List<string> keywordsList;

        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
        public List<string> KeyWordsList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Keywords))
                {
                    keywordsList = new List<string>();
                }
                else
                {
                    keywordsList = Keywords.Split(" ", StringSplitOptions.RemoveEmptyEntries).
                        Select(key => key.ToLower()).ToList();
                }
                return keywordsList;
            }

        }

        [Required]
        public string Keywords { get; set; }

        [Required]
        [RegularExpression(@"^(?:https?://|s?ftps?://)?(?!www | www\.)[A-Za-z0-9_-]+\.+[A-Za-z0-9.\/%&=\?_:;-]+$", ErrorMessage = "Url is not in correct format")]
        public string Url { get; set; }
    }
}
