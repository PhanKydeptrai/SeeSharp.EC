using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Domain.Entities.CustomerEntity;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class CustomerConfigurationForPostgreSQL : IEntityTypeConfiguration<Customer>
{

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(a => a.CustomerId);
        builder.Property(a => a.CustomerId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");
        
        builder.Property(a => a.CustomerStatus) 
            .IsRequired()
            // .HasConversion<EnumToStringConverter<CustomerStatus>>()
            .HasColumnType("varchar(20)");

        builder.Property(a => a.CustomerType)
            .IsRequired()
            // .HasConversion<EnumToStringConverter<CustomerType>>()
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
