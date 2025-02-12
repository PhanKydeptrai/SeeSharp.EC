using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations;

internal sealed class OrderDetailConfigurationForMySQL : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.OrderDetailId);
        builder.Property(x => x.OrderDetailId)
            .IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => OrderDetailId.FromString(x)
            )
            .HasColumnType("varchar(26)");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => OrderId.FromString(x)
            )
            .HasColumnType("varchar(26)");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => ProductId.FromString(x)
            )
            .HasColumnType("varchar(26)");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasConversion(
                x => x.Value,
                x => OrderDetailQuantity.FromInt(x)
            )
            .HasColumnType("int");

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasConversion(
                x => x.Value,
                x => OrderDetailUnitPrice.FromDecimal(x)
            )
            .HasColumnType("decimal");
    }
}
