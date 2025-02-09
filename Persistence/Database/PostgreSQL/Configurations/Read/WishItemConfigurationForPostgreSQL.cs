using Domain.Entities.Customers;
using Domain.Entities.Products;
using Domain.Entities.WishItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class WishItemReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<WishItem>
{
    public void Configure(EntityTypeBuilder<WishItem> builder)
    {
        builder.HasKey(x => x.WishItemId);
        builder.Property(x => x.WishItemId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new WishItemId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");
        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new CustomerId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new ProductId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

    }
}
