using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class ProductReadModelConfigurationForMySQL : IEntityTypeConfiguration<ProductReadModel>
{
    public void Configure(EntityTypeBuilder<ProductReadModel> builder)
    {
        builder.HasKey(a => a.ProductId);
        builder.Property(a => a.ProductId)
            .IsRequired()
            
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

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
            .HasColumnType("varchar(20)");

        builder.Property(a => a.CategoryId)
            .IsRequired()
            
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.HasMany(a => a.WishItemReadModels)
            .WithOne(a => a.ProductReadModel)
            .HasForeignKey(a => a.ProductId);

        builder.HasMany(a => a.OrderDetailReadModels)
            .WithOne(a => a.ProductReadModel)
            .HasForeignKey(a => a.ProductId);
    }
}
