using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class OrderReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderReadModel>
{
    public void Configure(EntityTypeBuilder<OrderReadModel> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.Total)
            .IsRequired()
            .HasColumnType("decimal");

        builder.Property(x => x.PaymentStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(x => x.OrderStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(x => x.OrderTransactionId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");
        
        // builder.Property(x => x.BillId)
        //     .IsRequired(false)
        //     .HasConversion(
        //         value => value.HasValue ? value.Value.ToGuid() : (Guid?)null,
        //         value => value.HasValue ? new Ulid(value.Value) : (Ulid?)null)
        //     .HasColumnType("uuid");

        builder.HasMany(x => x.OrderDetailReadModels)
            .WithOne(x => x.OrderReadModel)
            .HasForeignKey(x => x.OrderId);

        // builder.HasOne(a => a.BillReadModel)
        //     .WithOne(a => a.Order)
        //     .HasForeignKey<BillReadModel>(a => a.OrderId);

        builder.HasOne(a => a.OrderTransactionReadModel)
            .WithOne(a => a.OrderReadModel)
            .HasForeignKey<OrderTransactionReadModel>(a => a.OrderId);

    }
}
