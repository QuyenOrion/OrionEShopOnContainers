using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrionEShopOnContainers.Services.Catalog.API.Infrastructure.EntityConfigurations;

public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrand");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
                .IsRequired()
                .UseHiLo("catalog_brand_hilo");

        builder.Property(b => b.Brand)
                .IsRequired()
                .HasMaxLength(100);
    }
}
