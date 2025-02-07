using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Categories;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class CategoryConfigurationForPostgreSQL : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(a => a.CategoryId);
        builder.Property(a => a.CategoryId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToString(),
                value => new CategoryId(Ulid.Parse(value)))
            .HasColumnType("varchar(26)");

        builder.Property(a => a.CategoryName)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToString(),
                value => CategoryName.FromString(value))
            .HasColumnType("varchar(50)");

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
