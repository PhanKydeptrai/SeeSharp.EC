using Domain.Entities.Categories;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Write;

internal sealed class ProductConfigurationForMySQL : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(a => a.ProductId);
        builder.Property(a => a.ProductId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => ProductId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

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
            

        builder.Property(a => a.ProductPrice)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ProductPrice.FromDecimal(v))
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.ProductStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (ProductStatus)Enum.Parse(typeof(ProductStatus), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.CategoryId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => CategoryId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.HasMany(a => a.WishItems)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId);

        builder.HasMany(a => a.OrderDetails)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId);
    }
}
