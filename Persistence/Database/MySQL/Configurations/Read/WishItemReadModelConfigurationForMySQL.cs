using Domain.Database.PostgreSQL.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class WishItemReadModelConfigurationForMySQL : IEntityTypeConfiguration<WishItemReadModel>
{
    public void Configure(EntityTypeBuilder<WishItemReadModel> builder)
    {
        builder.HasKey(x => x.WishItemId);
        builder.Property(x => x.WishItemId)
            .IsRequired()
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

    }
}
