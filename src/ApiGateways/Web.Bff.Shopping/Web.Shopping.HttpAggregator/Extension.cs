using CatalogApi;
using Microsoft.AspNetCore.Authentication;

namespace OrionEShopOnContainers.Web.Shopping.HttpAggregator;

public static class Extension
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddScoped<IBasketService, BasketService>();

        services.AddGrpcClient<Basket.BasketClient>((services, o) =>
            {
                var basketApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcBasket;
                o.Address = new Uri(basketApi);
            })
            .AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var token = await httpContextAccessor.HttpContext?.GetTokenAsync("access_token");
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("Authorization", $"Bearer {token}");
                }
            })
            .ConfigureChannel(o => o.UnsafeUseInsecureChannelCallCredentials = true);

        services.AddScoped<ICatalogService, CatalogService>();

        services.AddGrpcClient<Catalog.CatalogClient>((services, o) =>
            {
                var catalogApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcCatalog;
                o.Address = new Uri(catalogApi);
            })
            .AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var token = await httpContextAccessor.HttpContext?.GetTokenAsync("access_token");
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("Authorization", $"Bearer {token}");
                }
            })
            .ConfigureChannel(o => o.UnsafeUseInsecureChannelCallCredentials = true);

        return services;
    }
}
