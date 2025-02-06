using Domain.Entities.OrderTransactions;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.EntityFrameworkCore.Extensions;
using Persistence.Converter;

namespace Persistence.Database.MySQL.Configuration;

internal class OrderTransactionConfigurationForMySQL : IEntityTypeConfiguration<OrderTransaction>
{
    public void Configure(EntityTypeBuilder<OrderTransaction> builder)
    {
        builder.HasKey(x => x.OrderTransactionId);
        builder.Property(a => a.OrderTransactionId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new OrderTransactionId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        builder.Property(a => a.PayerName)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value,
                v => PayerName.FromString(v)
            )
            .HasColumnType("varchar(50)")
            .ForMySQLHasCharset("utf8mb4");
        
        builder.Property(a => a.PayerEmail)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value,
                v => Email.NewEmail(v) //!FIXME
            )
            .HasColumnType("varchar(200)");

        builder.Property(a => a.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasColumnType("varchar(255)");

        builder.Property(a => a.PaymentMethod)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), v)
            )
            .HasColumnType("varchar(50)");

        builder.Property(a => a.IsVoucherUsed)
            .IsRequired()
            .HasColumnType("tinyint(1)");

        builder.Property(a => a.VoucherId)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value.ToString(),
                v => new VoucherId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        builder.Property(a => a.OrderId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.BillId)
            .IsRequired(false)
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.HasOne(a => a.Voucher)
            .WithMany(a => a.OrderTransactions)
            .HasForeignKey(a => a.VoucherId);
        
        
    }
}
