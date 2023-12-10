namespace Web.Shopping.HttpAggregator;

public static class Extension
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddScoped<IBasketService, BasketService>();

        services.AddGrpcClient<Basket.BasketClient>((services, o) =>
        {
            var basketApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcBasket;
            o.Address = new Uri(basketApi);
        });

        return services;
     }
}
