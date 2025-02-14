using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class VerificationTokenReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<VerificationTokenReadModel>
{
    public void Configure(EntityTypeBuilder<VerificationTokenReadModel> builder)
    {
        builder.HasKey(x => x.VerificationTokenId);
        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("uuid");

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
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("uuid");
    }
}
