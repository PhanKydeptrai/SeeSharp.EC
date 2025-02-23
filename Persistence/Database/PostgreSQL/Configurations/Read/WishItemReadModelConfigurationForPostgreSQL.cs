using Domain.Database.PostgreSQL.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class WishItemReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<WishItemReadModel>
{
    public void Configure(EntityTypeBuilder<WishItemReadModel> builder)
    {
        builder.HasKey(x => x.WishItemId);
        builder.Property(x => x.WishItemId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.ProductId)
            .IsRequired()
            
            .HasColumnType("uuid");

    }
}
