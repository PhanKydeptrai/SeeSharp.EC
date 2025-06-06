using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class OrderDetailConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.OrderDetailId);
        builder.Property(x => x.OrderDetailId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => OrderDetailId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => OrderId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.ProductVariantId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductVariantId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasConversion(
                x => x.Value,
                x => OrderDetailQuantity.FromInt(x))
            .HasColumnType("int");

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasConversion(
                x => x.Value,
                x => OrderDetailUnitPrice.FromDecimal(x))
            .HasColumnType("decimal");
    }
}
