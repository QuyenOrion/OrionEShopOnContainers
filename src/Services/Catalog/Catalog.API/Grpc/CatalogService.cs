using CatalogApi;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using static CatalogApi.Catalog;

namespace OrionEShopOnContainers.Services.Catalog.API.Grpc;

[Authorize]
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

        var catalog = await _catalogContext.CatalogItems.Include(c => c.CatalogType).Include(c => c.CatalogBrand).FirstOrDefaultAsync(item => item.Id == request.Id);

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
                PictureUri = catalog.PictureUri ?? catalog.PictureFileName,
                RestockThreshold = catalog.RestockThreshold,
                CatalogType = new CatalogApi.CatalogType
                {
                    Id = catalog.CatalogTypeId,
                    Type = catalog.CatalogType.Type
                },
                CatalogBrand = new CatalogApi.CatalogBrand
                {
                    Id = catalog.CatalogBrandId,
                    Name = catalog.CatalogBrand.Brand
                }
            };
        }

        context.Status = new Status(StatusCode.NotFound, $"Item with id {request.Id} do not exist");

        return null;
    }
}
