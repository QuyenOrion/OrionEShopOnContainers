﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrionEShopOnContainer.Services.Ordering.API.Infrastructure.Factories;

public class OrderingDbContextFactory : IDesignTimeDbContextFactory<OrderingContext>
{
    public OrderingContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
           .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
           .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables()
           .Build();

        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();

        optionsBuilder.UseNpgsql(config["ConnectionStrings:CatalogDb"], o => o.MigrationsAssembly("Ordering.API"));

        return new OrderingContext(optionsBuilder.Options);
    }
}
