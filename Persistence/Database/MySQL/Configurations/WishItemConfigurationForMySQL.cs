using Domain.Entities.WishItems;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Customers;
using Domain.Entities.Products;

namespace Persistence.Database.MySQL.Configurations;

internal sealed class WishItemConfigurationForMySQL : IEntityTypeConfiguration<WishItem>
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
