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
                v => v.Value.ToString(),
                v => CustomerVoucherId.FromString(v))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.VoucherId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => VoucherId.FromString(v))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => CustomerId.FromString(v))
            .HasColumnType("varchar(26)");

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
