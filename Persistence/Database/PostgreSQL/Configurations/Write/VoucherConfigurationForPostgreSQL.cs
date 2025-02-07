using NextSharp.Domain.Entities.VoucherEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class VoucherConfigurationForPostgreSQL : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(x => x.VoucherId);
        builder.Property(a => a.VoucherId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.VoucherName)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(a => a.VoucherCode)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(a => a.VoucherType)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (VoucherType)Enum.Parse(typeof(VoucherType), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.PercentageDiscount)
            .IsRequired()
            .HasColumnType("integer");
        
        builder.Property(a => a.MaximumDiscountAmount)
            .IsRequired()
            .HasColumnType("decimal");
        
        builder.Property(a => a.MinimumOrderAmount)
            .IsRequired()
            .HasColumnType("decimal");

        builder.Property(a => a.StartDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");
        
        builder.Property(a => a.ExpiredDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasColumnType("varchar(255)");

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v)
            )
            .HasColumnType("varchar(20)");
    }
}
