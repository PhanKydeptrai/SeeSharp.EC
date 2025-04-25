using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class VoucherConfigurationForPostgreSQL : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(x => x.VoucherId);
        builder.Property(a => a.VoucherId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => VoucherId.FromGuid(value))
            .HasColumnType("uuid");

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
            .HasColumnType("integer");

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
            .HasColumnType("date");

        builder.Property(a => a.ExpiredDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => VoucherDescription.FromString(v)
            )
            .HasColumnType("varchar(255)");

        builder.Property(a => a.Status)
            .IsRequired()
            .HasColumnType("integer");

        builder.HasData(Voucher.NewDirectDiscountVoucher(
                VoucherName.NewVoucherName("NEWUSER01"),
                VoucherCode.NewVoucherCode("NEWUSER01"),
                MaximumDiscountAmount.FromDecimal(10000),
                MinimumOrderAmount.FromDecimal(100000),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddMonths(12)),
                VoucherDescription.FromString("Voucher dành riêng cho khách hàng mới đăng ký tài khoản")));

    }
}
