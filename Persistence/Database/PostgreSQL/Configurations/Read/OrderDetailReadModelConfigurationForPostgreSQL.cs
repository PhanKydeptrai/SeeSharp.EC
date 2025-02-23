using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class OrderDetailReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<OrderDetailReadModel>
{
    public void Configure(EntityTypeBuilder<OrderDetailReadModel> builder)
    {
        builder.HasKey(x => x.OrderDetailId);
        builder.Property(x => x.OrderDetailId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.OrderId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.ProductId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal");
    }
}
