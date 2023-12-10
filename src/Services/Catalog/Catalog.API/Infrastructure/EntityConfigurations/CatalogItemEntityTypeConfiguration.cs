using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrionEShopOnContainers.Services.Catalog.API.Infrastructure.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("Catalog");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
                .IsRequired()
                .UseHiLo("catalog_hilo");

        builder.Property(ci => ci.Name)
                .IsRequired()
                .HasMaxLength(50);

        builder.Property(ci => ci.Price)
                .IsRequired();

        builder.HasOne(ci => ci.CatalogBrand)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogBrandId);

        builder.HasOne(ci => ci.CatalogType)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogTypeId);
    }
}
