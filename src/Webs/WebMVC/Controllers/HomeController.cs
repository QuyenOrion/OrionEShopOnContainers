using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using OrionEShopOnContainer.Webs.WebMVC.Models;

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
            var vm = new CatalogIndexViewModel
            {
                CatalogItems = new List<CatalogItem> { new() { Id = 1 }, new() { Id = 2 } }
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
