using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.MySQL.Configurations.Read;

internal sealed class OrderDetailReadModelConfigurationForMySQL : IEntityTypeConfiguration<OrderDetailReadModel>
{
    public void Configure(EntityTypeBuilder<OrderDetailReadModel> builder)
    {
        builder.HasKey(x => x.OrderDetailId);
        builder.Property(x => x.OrderDetailId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal");
    }
}
