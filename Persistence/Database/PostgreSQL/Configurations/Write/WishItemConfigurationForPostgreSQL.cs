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
                value => value.Value.ToGuid(),
                value => WishItemId.FromGuid(value))
            .HasColumnType("uuid");
        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => CustomerId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => ProductId.FromGuid(value))
            .HasColumnType("uuid");

    }
}
