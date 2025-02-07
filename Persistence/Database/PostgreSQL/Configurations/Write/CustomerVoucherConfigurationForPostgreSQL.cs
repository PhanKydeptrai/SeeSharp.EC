using NextSharp.Domain.Entities.CustomerVoucherEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class CustomerVoucherConfigurationForPostgreSQL : IEntityTypeConfiguration<CustomerVoucher>
{
    public void Configure(EntityTypeBuilder<CustomerVoucher> builder)
    {
        builder.HasKey(x => x.CustomerVoucherId);
        builder.Property(x => x.CustomerVoucherId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");
        
        builder.Property(x => x.VoucherId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");
                
        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("integer");

        //* Một voucher có nhiều customerVoucher
        builder.HasOne(x => x.Voucher)
            .WithMany(x => x.CustomerVouchers)
            .HasForeignKey(x => x.VoucherId);          
    }
}
