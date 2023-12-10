namespace OrionEShopOnContainers.Web.Shopping.HttpAggregator.Services;

public interface IBasketService
{
    Task<BasketData> GetByIdAsync(string id);

    Task UpdateAsync(BasketData currentBasket);
}

public class BasketService : IBasketService
{
    private readonly Basket.BasketClient _basketClient;
    private readonly ILogger<BasketService> _logger;

    public BasketService(Basket.BasketClient basketClient, ILogger<BasketService> logger)
    {
        _basketClient = basketClient;
        _logger = logger;
    }

    public async Task<BasketData> GetByIdAsync(string id)
    {
        _logger.LogDebug("grpc client created, request = {@id}", id);
        var response = await _basketClient.GetBasketByIdAsync(new BasketRequest { Id = id });
        _logger.LogDebug("grpc response {@response}", response);

        return MapToBasketData(response);
    }

    public async Task UpdateAsync(BasketData currentBasket)
    {
        _logger.LogDebug("Grpc update basket currentBasket {@currentBasket}", currentBasket);
        var request = MapToBasketRequest(currentBasket);
        _logger.LogDebug("Grpc update basket request {@request}", request);

        await _basketClient.UpdateBasketAsync(request);
    }

    private CustomerBasketRequest MapToBasketRequest(BasketData currentBasket)
    {
        if (currentBasket == null)
        {
            return null;
        }

        var basket = new CustomerBasketRequest
        {
            Buyerid = currentBasket.BuyerId
        };

        currentBasket.Items.ToList().ForEach(item =>
        {
            if (item.Id != null)
            {
                basket.Items.Add(new BasketItemResponse
                {
                    Id = item.Id,
                    Oldunitprice = (double)item.OldUnitPrice,
                    Pictureurl = item.PictureUrl,
                    Productid = item.ProductId,
                    Productname = item.ProductName,
                    Quantity = item.Quantity,
                    Unitprice = (double)item.UnitPrice
                });
            }
        });

        return basket;
    }

    private BasketData MapToBasketData(CustomerBasketResponse customerBasketResponse)
    {
        if (customerBasketResponse == null)
        {
            return null;
        }

        var basket = new BasketData
        {
            BuyerId = customerBasketResponse.Buyerid
        };

        customerBasketResponse.Items.ToList().ForEach(item =>
        {
            if (item.Id != null)
            {
                basket.Items.Add(new BasketDataItem
                {
                    Id = item.Id,
                    OldUnitPrice = (decimal)item.Oldunitprice,
                    PictureUrl = item.Pictureurl,
                    ProductId = item.Productid,
                    ProductName = item.Productname,
                    Quantity = item.Quantity,
                    UnitPrice = (decimal)item.Unitprice
                });
            }
        });

        return basket;
    }
}
