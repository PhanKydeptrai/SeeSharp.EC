using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class BillConfigurationForPostgreSQL : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(x => x.BillId);
        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => BillId.FromGuid(value))
            .HasColumnType("uuid");

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

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ");

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasColumnType("integer");
        
        builder.Property(x => x.BillPaymentStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(x => x.IsRated)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => IsRated.FromBoolean(value))
            .HasColumnType("boolean");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => FullName.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => PhoneNumber.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.SpecificAddress)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => SpecificAddress.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.Province)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => Province.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.District)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => District.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.Ward)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => Ward.FromString(value))
            .HasColumnType("text");

    }
}
