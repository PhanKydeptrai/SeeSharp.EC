using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class ProductVariantConfigurationForPostgreSQL : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(a => a.ProductVariantId);

        builder.Property(a => a.ProductVariantId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductVariantId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.VariantName)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => VariantName.FromString(v))
            .HasColumnType("varchar(50)");

        builder.Property(a => a.ProductId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.ProductVariantPrice)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ProductVariantPrice.FromDecimal(v))
            .HasColumnType("decimal(10,2)");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ProductVariantDescription.FromString(v))
            .HasColumnType("varchar(500)");

        builder.HasMany(a => a.WishItems)
            .WithOne(a => a.ProductVariant)
            .HasForeignKey(a => a.ProductVariantId);

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(500)");
    }
}
