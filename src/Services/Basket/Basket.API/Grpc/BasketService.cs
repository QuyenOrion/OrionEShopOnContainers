namespace GrpcBasket;

[Authorize]
public class BasketService : Basket.BasketBase
{
    private readonly IBasketRepository _repository;
    private readonly ILogger<BasketService> _logger;

    public BasketService(IBasketRepository repository, ILogger<BasketService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public override async Task<CustomerBasketResponse> GetBasketById(BasketRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Begin grpc call from method {Method} for basket id {Id}", context.Method, request.Id);

        var basket = await _repository.GetBasketAsync(request.Id);

        if (basket != null)
        {
            context.Status = new Status(StatusCode.OK, $"Basket with id {request.Id} do exist");

            return MapToCustomerBasketResponse(basket);
        }
        else
            context.Status = new Status(StatusCode.NotFound, $"Basket with id {request.Id} do not exist");

        return new CustomerBasketResponse { Buyerid = request.Id };
    }

    public override async Task<CustomerBasketResponse> UpdateBasket(CustomerBasketRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Begin grpc call BasketService.UpdateBasketAsync for buyer id {Buyerid}", request.Buyerid);

        var basket = MapToCustomerBasket(request);

        var response = await _repository.UpdateBasketAsync(basket);

        if (response != null)
            return MapToCustomerBasketResponse(basket);

        context.Status = new Status(StatusCode.NotFound, $"Basket with buyer id {request.Buyerid} do not exist");

        return new CustomerBasketResponse { Buyerid = request.Buyerid };
    }

    private CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket customerBasket)
    {
        var response = new CustomerBasketResponse
        {
            Buyerid = customerBasket.BuyerId
        };

        customerBasket.Items.ForEach(item => response.Items.Add(new BasketItemResponse
        {
            Id = item.Id,
            Oldunitprice = (double)item.OldUnitPrice,
            Pictureurl = item.PictureUrl,
            Productid = item.ProductId,
            Productname = item.ProductName,
            Quantity = item.Quantity,
            Unitprice = (double)item.UnitPrice
        }));

        return response;
    }

    private CustomerBasket MapToCustomerBasket(CustomerBasketRequest customerBasketRequest)
    {
        var response = new CustomerBasket
        {
            BuyerId = customerBasketRequest.Buyerid
        };

        customerBasketRequest.Items.ToList().ForEach(item => response.Items.Add(new BasketItem
        {
            Id = item.Id,
            OldUnitPrice = (decimal)item.Oldunitprice,
            PictureUrl = item.Pictureurl,
            ProductId = item.Productid,
            ProductName = item.Productname,
            Quantity = item.Quantity,
            UnitPrice = (decimal)item.Unitprice
        }));

        return response;
    }
}
