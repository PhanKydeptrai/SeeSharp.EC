using Domain.Database.PostgreSQL.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class BillReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<BillReadModel>
{
    public void Configure(EntityTypeBuilder<BillReadModel> builder)
    {
        builder.HasKey(x => x.BillId);
        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

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

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ");

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasColumnType("integer");
        
        builder.Property(x => x.BillPaymentStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(x => x.ShippingInformationId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.IsRated)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasColumnType("varchar(10)");

        builder.Property(x => x.SpecificAddress)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.Province)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.District)
            .IsRequired()
            .HasColumnType("text");

    }
}
