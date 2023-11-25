namespace OrionEShopOnContainer.Webs.WebMVC.Extensions;

internal static class Extensions
{
    public static void AddHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpClient<IBasketService, BasketService>();

        services.AddTransient<IIdentityParser<ApplicationUser>, IdentityParser>();
    }
}
