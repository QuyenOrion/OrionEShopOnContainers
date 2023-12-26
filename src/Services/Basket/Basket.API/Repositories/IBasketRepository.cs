namespace OrionEShopOnContainers.Services.Basket.API.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string customerId);

    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
}

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;
    private readonly ILogger<BasketRepository> _logger;

    public BasketRepository(ConnectionMultiplexer multiplexer, ILogger<BasketRepository> logger)
    {
        _database = multiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<CustomerBasket?> GetBasketAsync(string customerId)
    {
        var basketValue = await _database.StringGetAsync(customerId);
        if (basketValue.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CustomerBasket>(basketValue, JsonDefaults.CaseInsensitiveOptions);
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var created = await _database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket, JsonDefaults.CaseInsensitiveOptions));
           
        if(!created)
        {
            _logger.LogError("Problem occur persisting the item.");
            return null;
        }

        return await GetBasketAsync(basket.BuyerId);
    }
}
