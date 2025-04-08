using Domain.Entities.Bills;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class OrderTransactionConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderTransaction>
{
    public void Configure(EntityTypeBuilder<OrderTransaction> builder)
    {
        builder.HasKey(x => x.OrderTransactionId);
        builder.Property(a => a.OrderTransactionId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => OrderTransactionId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.PayerName)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value,
                v => PayerName.FromString(v))
            .HasColumnType("varchar(50)");

        builder.Property(a => a.PayerEmail)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value,
                v => Email.FromString(v))
            .HasColumnType("varchar(200)");

        builder.Property(a => a.Amount)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => AmountOfOrderTransaction.FromDecimal(v))
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => DescriptionOfOrderTransaction.FromString(v))
            .HasColumnType("varchar(255)");

        builder.Property(a => a.PaymentMethod)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.TransactionStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.IsVoucherUsed)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IsVoucherUsed.FromBoolean(v))
            .HasColumnType("boolean");

        builder.Property(a => a.VoucherId)
            .IsRequired(false)
            .HasConversion(
                value => value!.Value,
                value => VoucherId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => OrderId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.BillId)
            .IsRequired(false)
            .HasConversion(
                value => value!.Value,
                value => BillId.FromGuid(value))
            .HasColumnType("uuid");

        builder.HasOne(a => a.Voucher)
            .WithMany(a => a.OrderTransactions)
            .HasForeignKey(a => a.VoucherId);


    }

}
