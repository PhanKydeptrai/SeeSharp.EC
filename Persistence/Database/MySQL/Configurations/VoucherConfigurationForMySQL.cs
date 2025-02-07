using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.EntityFrameworkCore.Extensions;

namespace Persistence.Database.MySQL.Configurations;

internal sealed class VoucherConfigurationForMySQL : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(x => x.VoucherId);
        builder.Property(a => a.VoucherId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new VoucherId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

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
                v => PercentageDiscount.FromDecimal(v)
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
            .HasColumnType("varchar(255)")
            .ForMySQLHasCharset("utf8mb4");

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v)
            )
            .HasColumnType("varchar(20)");
    }
}
