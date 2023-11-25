using OrionEShopOnContainer.Webs.WebMVC.Models;

namespace OrionEShopOnContainer.Webs.WebMVC.Services
{
    public interface IBasketService
    {
        Task AddItemToBasket(ApplicationUser user, int productId);
    }

    public class BasketService : IBasketService
    {
        public Task AddItemToBasket(ApplicationUser user, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
