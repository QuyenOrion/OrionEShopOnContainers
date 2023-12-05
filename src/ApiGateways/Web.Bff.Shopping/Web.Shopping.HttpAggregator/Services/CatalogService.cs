namespace OrionEShopOnContainers.Web.Shopping.HttpAggregator.Services;

public interface ICatalogService
{
    Task<CatalogItem> GetCatalogItemAsync(int id);
}

public class CatalogService : ICatalogService
{
    public Task<CatalogItem> GetCatalogItemAsync(int id)
    {
        throw new NotImplementedException();
    }
}
