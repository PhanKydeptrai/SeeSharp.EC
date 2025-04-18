using Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Read;

internal sealed class ProductVariantReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<ProductVariantReadModel>
{
    public void Configure(EntityTypeBuilder<ProductVariantReadModel> builder)
    {
        builder.HasKey(a => a.ProductVariantId);

        builder.Property(a => a.ProductVariantId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.VariantName)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(a => a.ProductId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");
        
        builder.Property(a => a.ColorCode)
            .IsRequired()
            .HasColumnType("varchar(10)");

        builder.Property(a => a.ProductVariantPrice)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(a => a.ProductVariantStatus)
            .IsRequired()
            .HasColumnType("integer");
        
        builder.Property(a => a.IsBaseVariant)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(a => a.Description)
            .IsRequired()
            .HasColumnType("varchar(500)");


        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(500)");
        
        builder.HasMany(a => a.WishItemReadModels)
            .WithOne(a => a.ProductVariantReadModel)
            .HasForeignKey(a => a.ProductVariantId);

        builder.HasMany(a => a.OrderDetailReadModels)
            .WithOne(a => a.ProductVariantReadModel)
            .HasForeignKey(a => a.ProductVariantId);
            
        builder.HasOne(a => a.ProductReadModel)
            .WithMany(p => p.ProductVariantReadModels)
            .HasForeignKey(a => a.ProductId);
    }
}
