using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class CustomerVoucherConfigurationForPostgreSQL : IEntityTypeConfiguration<CustomerVoucher>
{
    public void Configure(EntityTypeBuilder<CustomerVoucher> builder)
    {
        builder.HasKey(x => x.CustomerVoucherId);
        builder.Property(x => x.CustomerVoucherId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => CustomerVoucherId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.VoucherId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => VoucherId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => CustomerId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CustomerVoucherQuantity.FromInt(v))
            .HasColumnType("integer");

        //* Một voucher có nhiều customerVoucher
        builder.HasOne(x => x.Voucher)
            .WithMany(x => x.CustomerVouchers)
            .HasForeignKey(x => x.VoucherId);
    }
}
