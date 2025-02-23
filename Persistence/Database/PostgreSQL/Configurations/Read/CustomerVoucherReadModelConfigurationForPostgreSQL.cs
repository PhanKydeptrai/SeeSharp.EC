using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class CustomerVoucherReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<CustomerVoucherReadModel>
{
    public void Configure(EntityTypeBuilder<CustomerVoucherReadModel> builder)
    {
        builder.HasKey(x => x.CustomerVoucherId);
        builder.Property(x => x.CustomerVoucherId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.VoucherId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            
            .HasColumnType("uuid");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("integer");

        //* Một voucher có nhiều customerVoucher
        builder.HasOne(x => x.VoucherReadModel)
            .WithMany(x => x.CustomerVoucherReadModels)
            .HasForeignKey(x => x.VoucherId);
    }
}
