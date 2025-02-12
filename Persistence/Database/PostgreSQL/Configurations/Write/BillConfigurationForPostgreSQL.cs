using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;
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
                value => value.ToString(),
                value => BillId.FromString(value))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.ToString(),
                value => OrderId.FromString(value))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.ToString(),
                value => CustomerId.FromString(value))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasConversion(
                value => value.ToString(),
                value => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), value))
            .HasColumnType("varchar(20)");

        builder.Property(x => x.ShippingInformationId)
            .IsRequired()
            .HasConversion(
                value => value.ToString(),
                value => ShippingInformationId.FromString(value))
            .HasColumnType("varchar(26)");

        builder.HasOne(a => a.ShippingInformation)
            .WithMany(a => a.Bills)
            .HasForeignKey(a => a.ShippingInformationId);


    }
}
