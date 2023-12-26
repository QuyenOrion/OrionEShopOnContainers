using CatalogApi;

namespace OrionEShopOnContainers.Web.Shopping.HttpAggregator.Services;

public interface ICatalogService
{
    Task<CatalogItem> GetCatalogItemAsync(int id);
}

public class CatalogService : ICatalogService
{
    private readonly Catalog.CatalogClient _catalogClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(Catalog.CatalogClient catalogClient, ILogger<CatalogService> logger)
    {
        _catalogClient = catalogClient;
        _logger = logger;
    }

    public async Task<CatalogItem> GetCatalogItemAsync(int id)
    {
        var request = new CatalogItemRequest { Id = id };
        _logger.LogDebug("Grpc client request {@request}", request);
        var response = await _catalogClient.GetItemByIdAsync(request);
        _logger.LogDebug("Grpc client response {@response}", response);
        return MapToCatalogItemResponse(response);
    }

    private CatalogItem MapToCatalogItemResponse(CatalogItemResponse catalogItemResponse)
    {
        return new CatalogItem
        {
            Id = catalogItemResponse.Id,
            Name = catalogItemResponse.Name,
            PictureUri = catalogItemResponse.PictureUri,
            Price = (decimal)catalogItemResponse.Price
        };
    }
}
