using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Domain.Entities.ProductEntity;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class ProductConfigurationForPostgreSQL : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(a => a.ProductId);
        builder.Property(a => a.ProductId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.ProductName)
            .IsRequired()
                .HasColumnType("varchar(50)");
        
        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(a => a.Description)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(a => a.ProductPrice)
            .IsRequired()
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
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.HasMany(a => a.WishItems)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId);
        
        builder.HasMany(a => a.OrderDetails)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId);
    }
}
