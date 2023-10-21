using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrionEShopOnContainer.Services.Ordering.Infrastructure.EntityConfigurations;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", OrderingContext.DEFAULT_SCHEMA);

        builder.HasKey(o => o.Id);

        builder.Ignore(o => o.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);

        builder
            .OwnsOne(o => o.Address, a =>
            {
                a.Property<int>("OrderId")
                .UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);
                a.WithOwner();
            });

        builder.Property<int?>("_buyerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BuyerId")
            .IsRequired(false);

        builder.Property<DateTime>("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderDate")
            .IsRequired();

        builder.Property<int>("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderStatusId")
            .IsRequired();

        builder.Property<int?>("_paymentMethodId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PaymentMethodId")
            .IsRequired(false);

        builder.Property<string>("_description")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Description")
            .IsRequired(false);

        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey("_paymentMethodId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Buyer>()
            .WithMany()
            .HasForeignKey("_buyerId")
            .IsRequired(false);

        builder.HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey("_orderStatusId")
            .IsRequired();
    }
}
