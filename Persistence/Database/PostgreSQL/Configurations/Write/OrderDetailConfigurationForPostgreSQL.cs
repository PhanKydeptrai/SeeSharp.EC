using NextSharp.Domain.Entities.OrderDetailEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class OrderDetailConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.OrderDetailId);
        builder.Property(x => x.OrderDetailId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("int");
        
        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal");       
    }
}
