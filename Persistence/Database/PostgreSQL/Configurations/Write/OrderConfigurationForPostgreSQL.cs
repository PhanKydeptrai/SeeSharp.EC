using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class OrderConfigurationForPostgreSQL : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => OrderId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => CustomerId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.Total)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => OrderTotal.FromDecimal(v))
            .HasColumnType("decimal");

        builder.Property(x => x.PaymentStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(x => x.OrderStatus)
            .IsRequired()
            .HasColumnType("integer");

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
