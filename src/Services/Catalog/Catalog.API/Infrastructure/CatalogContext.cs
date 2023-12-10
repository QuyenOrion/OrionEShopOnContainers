﻿namespace OrionEShopOnContainers.Services.Catalog.API.Infrastructure;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    public DbSet<CatalogBrand> CatalogBrands { get; set; } = default!;
    public DbSet<CatalogType> CatalogTypes { get; set; } = default!;
    public DbSet<CatalogItem> CatalogItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }
}
