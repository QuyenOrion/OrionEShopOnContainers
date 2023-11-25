using Microsoft.AspNetCore.Mvc;

namespace OrionEShopOnContainer.Webs.WebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;

        public CartController(IBasketService basketService, IIdentityParser<ApplicationUser> appUserParser)
        {
            _basketService = basketService;
            _appUserParser = appUserParser;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CatalogItem productDetails)
        {
            try
            {
                if(productDetails?.Id != null)
                {
                    var user = _appUserParser.Parse(HttpContext.User);
                    await _basketService.AddItemToBasket(user, productDetails.Id);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return RedirectToAction("Index", "Home", new { errorMsg = ViewBag.BasketInoperativeMsg });
        }

        private void HandleException(Exception ex)
        {
            ViewBag.BasketInoperativeMsg = $"Basket Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
