using Domain.Entities.Customers;
using Domain.Entities.Products;
using Domain.Entities.WishItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class WishItemConfigurationForPostgreSQL : IEntityTypeConfiguration<WishItem>
{
    public void Configure(EntityTypeBuilder<WishItem> builder)
    {
        builder.HasKey(x => x.WishItemId);
        builder.Property(x => x.WishItemId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => WishItemId.FromString(v))
            .HasColumnType("varchar(26)");
        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => CustomerId.FromString(v)
            )
            .HasColumnType("varchar(26)");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => ProductId.FromString(v)
            )
            .HasColumnType("varchar(26)");

    }
}
