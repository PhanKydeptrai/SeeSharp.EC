using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class CategoryConfigurationForPostgreSQL : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(a => a.CategoryId);
        
        builder.HasIndex(a => a.CategoryName) //Unique Index
            .IsUnique();

        builder.HasIndex(a => a.CategoryStatus)
            .IsUnique();
            
        builder.Property(a => a.CategoryId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => CategoryId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.CategoryName)
            .IsRequired()
            .HasConversion(
                value => value.Value,
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

        builder.Property(a => a.IsDefault)
            .IsRequired()
            .HasColumnType("boolean");

        // Seed Data
        builder.HasData(
            Category.FromExisting(
                CategoryId.FromString("019546cc-2909-1710-9a1b-36df36d9a7ae"),
                CategoryName.FromString("General"),
                string.Empty,
                CategoryStatus.Available,
                true));

        builder.HasMany(a => a.Products) // One to Many
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId);
    }

}
