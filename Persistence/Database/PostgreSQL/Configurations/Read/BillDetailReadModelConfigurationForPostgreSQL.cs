using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Read;

internal sealed class BillDetailReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<BillDetailReadModel>
{
    public void Configure(EntityTypeBuilder<BillDetailReadModel> builder)
    {
        builder.HasKey(x => x.BillDetailId);
        
        builder.Property(x => x.BillDetailId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.VariantName)
            .IsRequired()
            .HasColumnType("text");
        
        builder.Property(x => x.ProductVariantPrice)
            .IsRequired()
            .HasColumnType("decimal");
        
        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal");

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.BillDetailQuantity)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(x => x.ColorCode)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.ProductVariantDescription)
            .IsRequired()
            .HasColumnType("text");

        builder.HasOne(x => x.Bill)
            .WithMany(x => x.BillDetails)
            .HasForeignKey(x => x.BillId);
    }
} 