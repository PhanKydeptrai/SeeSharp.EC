using Domain.Entities.Customers;
using Domain.Entities.Products;
using Domain.Entities.WishItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Write;
internal sealed class WishItemConfigurationForMySQL : IEntityTypeConfiguration<WishItem>
{
    public void Configure(EntityTypeBuilder<WishItem> builder)
    {
        builder.HasKey(x => x.WishItemId);
        builder.Property(x => x.WishItemId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => WishItemId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => CustomerId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => ProductId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

    }
}
