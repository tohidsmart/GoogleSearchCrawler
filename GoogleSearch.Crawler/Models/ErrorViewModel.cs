using System;

namespace GoogleSearch.Crawler.Models
{
    public class ErrorViewModel
    {
        public string ExceptionMessage { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}