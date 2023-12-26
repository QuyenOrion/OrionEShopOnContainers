using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrionEShopOnContainer.Webs.WebMVC.Controllers;
public class CatalogController : Controller
{
    public IActionResult Index()
    {
        var catalogItems = new List<CatalogItem>
        {
            new()
            {
                Id = 1,
                CatalogTypeId = 2,
                CatalogBrandId = 2,
                Description = ".NET Bot Black Hoodie",
                Name = ".NET Bot Black Hoodie",
                Price = 19.5M
            },
            new() { Id = 2, CatalogTypeId = 1, CatalogBrandId = 2, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M },
        };

        var vm = new CatalogIndexViewModel { CatalogItems = catalogItems };

        return View(vm);
    }
}
