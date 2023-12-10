using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrionEShopOnContainers.Services.Catalog.API.Infrastructure.EntityConfigurations;

public class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
                .IsRequired()
                .UseHiLo("catalog_type_hilo");

        builder.Property(b => b.Type)
                .IsRequired()
                .HasMaxLength(100);
    }
}