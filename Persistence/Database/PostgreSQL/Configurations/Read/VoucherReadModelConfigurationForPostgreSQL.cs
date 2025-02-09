using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Persistence.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Read;

internal sealed class VoucherReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<VoucherReadModel>
{
    public void Configure(EntityTypeBuilder<VoucherReadModel> builder)
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
            .HasColumnType("varchar(20)");
    }
}
