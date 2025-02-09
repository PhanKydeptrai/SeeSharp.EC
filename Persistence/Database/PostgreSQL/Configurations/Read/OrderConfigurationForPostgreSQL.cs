using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class OrderReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new OrderId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new CustomerId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.Total)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => OrderTotal.FromDecimal(v))
            .HasColumnType("decimal");

        builder.Property(x => x.PaymentStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (OrderPaymentStatus)Enum.Parse(typeof(OrderPaymentStatus), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(x => x.OrderStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(x => x.OrderTransactionId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new OrderTransactionId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.HasMany(x => x.OrderDetails)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);

        builder.HasOne(a => a.Bill)
            .WithOne(a => a.Order)
            .HasForeignKey<Bill>(a => a.OrderId);

        builder.HasOne(a => a.OrderTransaction)
            .WithOne(a => a.Order)
            .HasForeignKey<OrderTransaction>(a => a.OrderId);

    }
}
