namespace OrionEShopOnContainers.Web.Shopping.HttpAggregator.Controllers;

[Route("api/v1/[controller]")]
[Authorize]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ICatalogService _catalogService;

    public BasketController(IBasketService basketService, ICatalogService catalogService)
    {
        _basketService = basketService;
        _catalogService = catalogService;
    }

    [HttpPost]
    [Route("items")]
    public async Task<IActionResult> AddItemToBasketAsync([FromBody] AddBasketItemRequest basketItem)
    {
        if(basketItem == null || basketItem.Quantity == 0)
        {
            return BadRequest("Invalid payload");
        }

        var item = await _catalogService.GetCatalogItemAsync(basketItem.CatalogItemId);

        if(item == null)
        {
            return BadRequest($"Basket with id {basketItem.CatalogItemId} not found.");
        }

        var currentBasket = (await _basketService.GetByIdAsync(basketItem.BasketId)) ?? new BasketData(basketItem.BasketId);

        var product = currentBasket.Items.FirstOrDefault(p => p.ProductId == item.Id);

        if(product == null)
        {
            currentBasket.Items.Add(new BasketDataItem()
            {
                ProductId = item.Id,
                ProductName = item.Name,
                Quantity = basketItem.Quantity,
                PictureUrl = item.PictureUri,
                Id = Guid.NewGuid().ToString(),
            });
        }
        else
        {
            product.Quantity += basketItem.Quantity;
        }

        await _basketService.UpdateAsync(currentBasket);

        return Ok();
    }
}