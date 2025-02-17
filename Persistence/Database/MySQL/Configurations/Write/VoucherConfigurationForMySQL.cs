using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Write;

internal sealed class VoucherConfigurationForMySQL : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(x => x.VoucherId);
        builder.Property(a => a.VoucherId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => VoucherId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(a => a.VoucherName)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => VoucherName.FromString(v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.VoucherCode)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => VoucherCode.FromString(v)
            )
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
            .HasConversion(
                v => v.Value,
                v => PercentageDiscount.FromInt(v)
            )
            .HasColumnType("integer");

        builder.Property(a => a.MaximumDiscountAmount)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => MaximumDiscountAmount.FromDecimal(v)
            )
            .HasColumnType("decimal");

        builder.Property(a => a.MinimumOrderAmount)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => MinimumOrderAmount.FromDecimal(v)
            )
            .HasColumnType("decimal");

        builder.Property(a => a.StartDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.ExpiredDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => VoucherDescription.FromString(v)
            )
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
