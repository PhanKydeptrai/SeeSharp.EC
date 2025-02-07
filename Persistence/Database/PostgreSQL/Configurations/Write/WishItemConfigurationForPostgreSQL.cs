using NextSharp.Domain.Entities.WishItemEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class WishItemConfigurationForPostgreSQL : IEntityTypeConfiguration<WishItem>
{
    public void Configure(EntityTypeBuilder<WishItem> builder)
    {
        builder.HasKey(x => x.WishItemId);
        builder.Property(x => x.WishItemId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");
        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

    }
}
