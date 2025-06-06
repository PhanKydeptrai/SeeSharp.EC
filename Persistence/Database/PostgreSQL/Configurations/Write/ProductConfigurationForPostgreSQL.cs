using Domain.Entities.Categories;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class ProductConfigurationForPostgreSQL : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(a => a.ProductId);
        builder.Property(a => a.ProductId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.ProductName)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ProductName.FromString(v))
            .HasColumnType("varchar(50)");

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(a => a.Description)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(a => a.ProductStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.CategoryId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => CategoryId.FromGuid(value))
            .HasColumnType("uuid");

        builder.HasMany(a => a.ProductVariants)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId);
    }
}
