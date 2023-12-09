namespace OrionEShopOnContainers.Web.Shopping.HttpAggregator.Services;

public interface IBasketService
{
    Task<BasketData> GetByIdAsync(string id);

    Task<BasketData> UpdateAsync(BasketData currentBasket);
}

public class BasketService : IBasketService
{
    //private readonly Grpc

    public BasketService()
    {
            
    }

    public Task<BasketData> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<BasketData> UpdateAsync(BasketData currentBasket)
    {
        throw new NotImplementedException();
    }
}
