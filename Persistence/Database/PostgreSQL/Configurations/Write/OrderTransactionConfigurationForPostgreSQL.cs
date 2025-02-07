using NextSharp.Domain.Entities.OrderTransactionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class OrderTransactionConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderTransaction>
{
    public void Configure(EntityTypeBuilder<OrderTransaction> builder)
    {
        builder.HasKey(x => x.OrderTransactionId);
        builder.Property(a => a.OrderTransactionId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.PayerName)
            .IsRequired(false)
            .HasColumnType("varchar(50)");
        
        builder.Property(a => a.PayerEmail)
            .IsRequired(false)
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
            .HasColumnType("boolean");

        builder.Property(a => a.VoucherId)
            .IsRequired(false)
            .HasConversion<UlidToStringConverter>()
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
