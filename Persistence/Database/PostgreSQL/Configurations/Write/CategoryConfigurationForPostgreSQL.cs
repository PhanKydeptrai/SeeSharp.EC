using NextSharp.Domain.Entities.CategoryEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class CategoryConfigurationForPostgreSQL : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(a => a.CategoryId);
        builder.Property(a => a.CategoryId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.CategoryName)
            .IsRequired()
                .HasColumnType("varchar(50)");

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(200)");

        builder.Property(a => a.CategoryStatus)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.HasMany(a => a.Products) // One to Many
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId);
    }

}
