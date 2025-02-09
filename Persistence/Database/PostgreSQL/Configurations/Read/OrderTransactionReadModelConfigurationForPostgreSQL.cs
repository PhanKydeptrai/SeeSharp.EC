using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Persistence.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class OrderTransactionReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderTransactionReadModel>
{
    public void Configure(EntityTypeBuilder<OrderTransactionReadModel> builder)
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

        builder.HasOne(a => a.VoucherReadModel)
            .WithMany(a => a.OrderTransactionReadModels)
            .HasForeignKey(a => a.VoucherId);


    }

}
