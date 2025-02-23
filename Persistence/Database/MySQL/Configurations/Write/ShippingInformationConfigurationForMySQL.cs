using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Write;

internal sealed class ShippingInformationConfigurationForMySQL : IEntityTypeConfiguration<ShippingInformation>
{
    public void Configure(EntityTypeBuilder<ShippingInformation> builder)
    {
        builder.HasKey(x => x.ShippingInformationId);
        builder.Property(x => x.ShippingInformationId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ShippingInformationId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => CustomerId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => FullName.FromString(v)
            )
            .HasColumnType("varchar(50)");

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => PhoneNumber.FromString(v)
            )
            .HasColumnType("varchar(10)");

        builder.Property(x => x.IsDefault)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IsDefault.FromBoolean(v)
            )
            .HasColumnType("tinyint(1)");

        builder.Property(x => x.SpecificAddress)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => SpecificAddress.FromString(v)
            )
            .HasColumnType("varchar(50)");

        builder.Property(x => x.Province)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => Province.FromString(v)
            )
            .HasColumnType("varchar(50)");

        builder.Property(x => x.District)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => District.FromString(v)
            )
            .HasColumnType("varchar(50)");

        builder.Property(x => x.Ward)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => Ward.FromString(v)
            )
            .HasColumnType("varchar(50)");
    }
}
