using Domain.Entities.BillDetails;
using Domain.Entities.Bills;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;


internal sealed class BillDetailConfigurationForPostgreSQL : IEntityTypeConfiguration<BillDetail>
{ 
    public void Configure(EntityTypeBuilder<BillDetail> builder)
    {
        builder.HasKey(x => x.BillDetailId);

        builder.Property(x => x.BillDetailId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => BillDetailId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => BillId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductName.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.VariantName)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => VariantName.FromString(value))
            .HasColumnType("text");
        
        builder.Property(x => x.ProductVariantPrice)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductVariantPrice.FromDecimal(value))
            .HasColumnType("decimal");
        
        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => BillDetailUnitPrice.FromDecimal(value))
            .HasColumnType("decimal");

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasConversion(
                value => value,
                value => value)
            .HasColumnType("text");

        builder.Property(x => x.BillDetailQuantity)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => BillDetailQuantity.NewOrderDetailQuantity(value))
            .HasColumnType("integer");

        builder.Property(x => x.ColorCode)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ColorCode.FromString(value))
            .HasColumnType("text");

        builder.Property(x => x.ProductVariantDescription)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => ProductVariantDescription.FromString(value))
            .HasColumnType("text");

    }
}
