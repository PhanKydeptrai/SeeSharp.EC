using NextSharp.Domain.Entities.BillEntity;
using NextSharp.Domain.Entities.OrderEntity;
using NextSharp.Domain.Entities.OrderTransactionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;
using System;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class OrderConfigurationForPostgreSQL : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.Total)
            .IsRequired()
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
            .HasConversion<UlidToStringConverter>()
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
