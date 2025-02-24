using Domain.Entities.Bills;
using Domain.Entities.OrderTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class OrderReadModelConfigurationForMySQL : IEntityTypeConfiguration<OrderReadModel>
{
    public void Configure(EntityTypeBuilder<OrderReadModel> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.Total)
            .IsRequired()
            .HasColumnType("decimal");

        builder.Property(x => x.PaymentStatus)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(x => x.OrderStatus)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(x => x.OrderTransactionId)
            .IsRequired()
            
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.HasMany(x => x.OrderDetailReadModels)
            .WithOne(x => x.OrderReadModel)
            .HasForeignKey(x => x.OrderId);

        builder.HasOne(a => a.BillReadModel)
            .WithOne(a => a.Order)
            .HasForeignKey<BillReadModel>(a => a.OrderId);

        builder.HasOne(a => a.OrderTransactionReadModel)
            .WithOne(a => a.OrderReadModel)
            .HasForeignKey<OrderTransactionReadModel>(a => a.OrderId);

    }
}
