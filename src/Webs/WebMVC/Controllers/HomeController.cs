using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using OrionEShopOnContainer.Webs.WebMVC.Models;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace OrionEShopOnContainer.Webs.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CallApi()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = await client.GetStringAsync("https://localhost:44381/identity");
            ViewBag.Json = JArray.Parse(content).ToString();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}