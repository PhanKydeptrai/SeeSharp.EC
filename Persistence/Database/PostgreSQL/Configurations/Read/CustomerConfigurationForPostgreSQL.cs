using Domain.Entities.Customers;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class CustomerReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<Customer>
{

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(a => a.CustomerId);
        builder.Property(a => a.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new CustomerId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new UserId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.Property(a => a.CustomerStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (CustomerStatus)Enum.Parse(typeof(CustomerStatus), v))
            .HasColumnType("varchar(20)");

        builder.Property(a => a.CustomerType)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (CustomerType)Enum.Parse(typeof(CustomerType), v))
            .HasColumnType("varchar(20)");


        builder.HasMany(a => a.ShippingInformations)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.CustomerVouchers)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.WishItems)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.Orders)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.Feedbacks)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);

        builder.HasMany(a => a.Bills)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId);
    }
}
