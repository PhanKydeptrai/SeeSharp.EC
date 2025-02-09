using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Persistence.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class VerificationTokenReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<VerificationTokenReadModel>
{
    public void Configure(EntityTypeBuilder<VerificationTokenReadModel> builder)
    {
        builder.HasKey(x => x.VerificationTokenId);
        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.Temporary)
            .IsRequired(false)
            .HasColumnType("varchar(64)");

        builder.Property(a => a.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.ExpiredDate)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");
    }
}
