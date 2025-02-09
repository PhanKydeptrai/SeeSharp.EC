using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Persistence.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Read;

internal sealed class OrderDetailReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderDetailReadModel>
{
    public void Configure(EntityTypeBuilder<OrderDetailReadModel> builder)
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
