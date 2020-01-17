using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoogleSearch.Crawler.Models;
using MediatR;
using System.Threading;
using System.Net.Http;
using HtmlAgilityPack;
using System.Linq;
using GoogleSearch.Crawler.Services.Model;
using GoogleSearch.Crawler.Entities;
using System.Buffers;
using System.IO;
using System.Text;

namespace GoogleSearch.Crawler.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMediator mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(SearchRequest request)
        {
            if (!TryValidateModel(request))
            {
                return View("Index");
            }
            var result = await mediator.Send(request);
            return View(result);
        }



       
        public async Task<IActionResult> Error()
        {

            Stream stream = Response.Body;
            string content = string.Empty;
            using (StreamReader readStream = new StreamReader(stream, Encoding.UTF8))
            {
                content = await readStream.ReadToEndAsync();
            }


            return View(new ErrorResult { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExceptionMessage = content });
        }
    }
}
