using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using OrionEShopOnContainer.Webs.WebMVCNew.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OrionEShopOnContainer.Webs.WebMVCNew.Services;

public interface IBasketService
{
    Task AddItemToBasket(ApplicationUser user, int productId);
}

public class BasketService : IBasketService
{
    private readonly ILogger<BasketService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _purchaseUrl;

    public BasketService(ILogger<BasketService> logger, IOptions<AppSettings> appsettings, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _purchaseUrl = appsettings.Value.PurchaseUrl;
    }

    public async Task AddItemToBasket(ApplicationUser user, int productId)
    {
        var uri = API.Purchase.AddItemToBasket(_purchaseUrl);

        var newItem = new
        {
            CatalogItemId = productId,
            BasketId = user.Id,
            Quantity = 1
        };

        var basketContent = new StringContent(JsonSerializer.Serialize(newItem), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, basketContent);
    }
}
