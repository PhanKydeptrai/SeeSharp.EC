using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations;

internal sealed class BillConfigurationForMySQL : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(x => x.BillId);
        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => BillId.FromGuid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("UUID()");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => OrderId.FromGuid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("UUID()");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => CustomerId.FromGuid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("UUID()");

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasConversion(
                value => value.ToString(),
                value => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), value)
            )
            .HasColumnType("varchar(20)");

        builder.Property(x => x.ShippingInformationId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => ShippingInformationId.FromGuid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("UUID()");

        builder.HasOne(a => a.ShippingInformation)
            .WithMany(a => a.Bills)
            .HasForeignKey(a => a.ShippingInformationId);
    }
}
