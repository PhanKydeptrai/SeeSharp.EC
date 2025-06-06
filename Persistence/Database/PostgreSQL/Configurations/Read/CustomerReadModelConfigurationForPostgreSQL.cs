using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class CustomerReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<CustomerReadModel>
{

    public void Configure(EntityTypeBuilder<CustomerReadModel> builder)
    {
        builder.HasKey(a => a.CustomerId);
        builder.Property(a => a.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        // builder.Property(a => a.CustomerStatus)
        //     .IsRequired()
        //     .HasColumnType("integer");

        builder.Property(a => a.CustomerType)
            .IsRequired()
            .HasColumnType("integer");


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
