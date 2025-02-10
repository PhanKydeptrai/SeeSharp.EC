using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class BillReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<BillReadModel>
{
    public void Configure(EntityTypeBuilder<BillReadModel> builder)
    {
        builder.HasKey(x => x.BillId);
        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(x => x.ShippingInformationId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.HasOne(a => a.ShippingInformation)
            .WithMany(a => a.Bills)
            .HasForeignKey(a => a.ShippingInformationId);


    }
}
