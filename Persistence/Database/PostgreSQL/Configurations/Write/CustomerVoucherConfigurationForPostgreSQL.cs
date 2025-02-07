using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Vouchers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Customers;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class CustomerVoucherConfigurationForPostgreSQL : IEntityTypeConfiguration<CustomerVoucher>
{
    public void Configure(EntityTypeBuilder<CustomerVoucher> builder)
    {
        builder.HasKey(x => x.CustomerVoucherId);
        builder.Property(x => x.CustomerVoucherId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new CustomerVoucherId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");
        
        builder.Property(x => x.VoucherId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new VoucherId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new CustomerId(Ulid.Parse(v)))
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
