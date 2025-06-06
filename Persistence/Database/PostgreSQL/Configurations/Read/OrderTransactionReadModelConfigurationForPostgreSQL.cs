using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class OrderTransactionReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderTransactionReadModel>
{
    public void Configure(EntityTypeBuilder<OrderTransactionReadModel> builder)
    {
        builder.HasKey(x => x.OrderTransactionId);
        builder.Property(a => a.OrderTransactionId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

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

        builder.Property(a => a.TransactionStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.IsVoucherUsed)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(a => a.VoucherId)
            .IsRequired(false)
            .HasConversion(
                value => value.HasValue ? value.Value.ToGuid() : (Guid?)null,
                value => value.HasValue ? new Ulid(value.Value) : (Ulid?)null)
            .HasColumnType("uuid");

        builder.Property(a => a.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.BillId)
            .IsRequired(false)
            .HasConversion(
                value => value.HasValue ? value.Value.ToGuid() : (Guid?)null,
                value => value.HasValue ? new Ulid(value.Value) : (Ulid?)null)
            .HasColumnType("uuid");

        builder.HasOne(a => a.VoucherReadModel)
            .WithMany(a => a.OrderTransactionReadModels)
            .HasForeignKey(a => a.VoucherId)
            .IsRequired(false);;


    }

}
