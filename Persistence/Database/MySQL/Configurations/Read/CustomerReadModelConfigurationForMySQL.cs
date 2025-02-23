using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class CustomerReadModelConfigurationForMySQL : IEntityTypeConfiguration<CustomerReadModel>
{

    public void Configure(EntityTypeBuilder<CustomerReadModel> builder)
    {
        builder.HasKey(a => a.CustomerId);
        builder.Property(a => a.CustomerId)
            .IsRequired()
            
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(a => a.UserId)
            .IsRequired()
            
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(a => a.CustomerStatus)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(a => a.CustomerType)
            .IsRequired()
            .HasColumnType("varchar(20)");


        builder.HasMany(a => a.ShippingInformations)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.CustomerVoucherReadModels)
            .WithOne(a => a.CustomerReadModel)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.WishItems)
            .WithOne(a => a.CustomerReadModel)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.OrderReadModels)
            .WithOne(a => a.CustomerReadModel)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.FeedbackReadModels)
            .WithOne(a => a.CustomerReadModel)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.BillReadModels)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);
    }
}
