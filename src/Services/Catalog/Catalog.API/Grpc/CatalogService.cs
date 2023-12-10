using CatalogApi;
using Grpc.Core;
using static CatalogApi.Catalog;

namespace OrionEShopOnContainers.Services.Catalog.API.Grpc;

public class CatalogService : CatalogBase
{
    private readonly CatalogContext _catalogContext;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(ILogger<CatalogService> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public override async Task<CatalogItemResponse> GetItemById(CatalogItemRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Begin grpc call from method {Method} for catalog item id {Id}", context.Method, request.Id);
        
        if (request.Id <= 0)
        {
            context.Status = new Status(StatusCode.FailedPrecondition, $"Id must be > 0 (received {request.Id})");
            return null;
        }

        var catalog = await _catalogContext.CatalogItems.FirstOrDefaultAsync(item => item.Id == request.Id);

        if (catalog != null)
        {
            context.Status = new Status(StatusCode.OK, $"Item with id {request.Id} do exist");

            return new CatalogItemResponse
            {
                Id = catalog.Id,
                AvailableStock = catalog.AvailableStock,
                Description = catalog.Description,
                MaxStockThreshold = catalog.MaxStockThreshold,
                Name = catalog.Name,
                OnReorder = catalog.OnReorder,
                PictureFileName = catalog.PictureFileName,
                Price = (double)catalog.Price,
                PictureUri = catalog.PictureUri,
                RestockThreshold = catalog.RestockThreshold,
            };
        }

        context.Status = new Status(StatusCode.NotFound, $"Item with id {request.Id} do not exist");

        return null;
    }
}
