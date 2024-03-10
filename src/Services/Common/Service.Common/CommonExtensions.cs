using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace OrionEShopOnContainers.Services.Service.Common;

public static class CommonExtensions
{
    public static void UseServiceDefault(this WebApplication app)
    {
        if(!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.MapDefaultHealthChecks();
    }

    public static void MapDefaultHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = (check) => !check.Name.Contains("self"),
        });
    }   
}