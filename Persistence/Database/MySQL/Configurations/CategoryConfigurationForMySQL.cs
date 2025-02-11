using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.EntityFrameworkCore.Extensions;

namespace Persistence.Database.MySQL.Configurations;

internal sealed class CategoryConfigurationForMySQL : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(a => a.CategoryId);
        builder.Property(a => a.CategoryId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToString(),
                value => CategoryId.FromString(value)
            )
            .HasColumnType("varchar(26)");

        builder.Property(a => a.CategoryName)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToString(),
                value => CategoryName.FromString(value)
            )
            .HasColumnType("varchar(50)")
            .ForMySQLHasCharset("utf8mb4");

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(200)");

        builder.Property(a => a.CategoryStatus)
            .IsRequired()
            .HasConversion(
                value => value.ToString(),
                value => (CategoryStatus)Enum.Parse(typeof(CategoryStatus), value))
            .HasColumnType("varchar(20)");

        builder.HasMany(a => a.Products) // One to Many
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId);
    }
}
