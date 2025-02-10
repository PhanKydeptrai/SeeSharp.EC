using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class CategoryReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<CategoryReadModel>
{
    public void Configure(EntityTypeBuilder<CategoryReadModel> builder)
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

        builder.HasMany(a => a.ProductReadModels) // One to Many
            .WithOne(a => a.CategoryReadModel)
            .HasForeignKey(a => a.CategoryId);
    }

}
