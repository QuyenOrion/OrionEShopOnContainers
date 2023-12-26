using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OrionEShopOnContainer.Webs.WebMVCNew.Models;
using OrionEShopOnContainer.Webs.WebMVCNew.Services;
using System.IdentityModel.Tokens.Jwt;

namespace OrionEShopOnContainer.Webs.WebMVCNew.Extensions;

internal static class Extensions
{
    public static void AddHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient<IBasketService, BasketService>();

        services.AddTransient<IIdentityParser<ApplicationUser>, IdentityParser>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityUrl");
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.RequireHttpsMetadata = false;
                options.CallbackPath = "/signin-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";
                options.SignedOutRedirectUri = "https://localhost:44321/signout-callback-oidc";
                options.ConfigurationManager = new Microsoft.IdentityModel.Protocols.ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{options.Authority}/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever { RequireHttps = false }
                    );
                options.Validate();
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("email");
                options.Scope.Add("myclaim");
                options.ClaimActions.MapUniqueJsonKey("myclaim", "myclaim");
                options.Scope.Add("api1");
                options.Scope.Add("offline_access");

                options.SaveTokens = true;
            });
    }
}
