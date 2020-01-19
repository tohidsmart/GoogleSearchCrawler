using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GoogleSearch.Crawler.Services.Model;
using GoogleSearch.Crawler.Entities;
using System.IO;
using System.Text;

namespace GoogleSearch.Crawler.Controllers
{

    public class HomeController : Controller
    {

        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        /// <summary>
        /// This Action show the default search page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// THis Action passes the Search request to search handler and displays the result
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
