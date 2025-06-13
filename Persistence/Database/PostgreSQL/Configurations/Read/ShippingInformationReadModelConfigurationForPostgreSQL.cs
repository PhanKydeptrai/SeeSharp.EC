using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class ShippingInformationReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<ShippingInformationReadModel>
{
    public void Configure(EntityTypeBuilder<ShippingInformationReadModel> builder)
    {
        builder.HasKey(x => x.ShippingInformationId);
        builder.Property(x => x.ShippingInformationId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasColumnType("varchar(10)");

        builder.Property(x => x.IsDefault)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(x => x.SpecificAddress)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(x => x.Province)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(x => x.District)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(x => x.Ward)
            .IsRequired()
            .HasColumnType("varchar(50)");
    }
}
