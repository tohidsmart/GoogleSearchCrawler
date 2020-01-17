using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSearch.Crawler.Entities
{
    public class ErrorResult
    {
        public string ExceptionMessage { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
