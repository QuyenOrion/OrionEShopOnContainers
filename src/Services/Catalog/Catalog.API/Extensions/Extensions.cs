using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using OrionEShopOnContainers.Services.Catalog.API.Infrastructure;

namespace OrionEShopOnContainers.Services.Catalog.API.Extensions;

public static class Extensions
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        static void ConfigurationOptions(NpgsqlDbContextOptionsBuilder sqlOptions)
        {
            sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
        }

        string connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrEmpty(connectionString))
        {
            var postgresHost = configuration.GetConnectionString("PostgresHost");
            var database = configuration.GetConnectionString("PostgresDb");
            var username = configuration.GetConnectionString("PostgresUser");
            var password = configuration.GetConnectionString("PostgresPassword");
            var options = configuration.GetConnectionString("PostgresOptions");
            connectionString = $"Host={postgresHost};Database={database};Username={username};Password={password};{options}";
        }

        services.AddDbContext<CatalogContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("CatalogDb"), ConfigurationOptions);
        });

        return services;
    }
}
