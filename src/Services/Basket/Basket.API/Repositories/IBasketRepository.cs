namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    }

    public class BasketRepository : IBasketRepository
    {
        public Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            throw new NotImplementedException();
        }
    }
}
