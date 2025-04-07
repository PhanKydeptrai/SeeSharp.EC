using Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Read;

internal sealed class VerificationTokenReadModelConfigurationForPostgreSQL 
    : IEntityTypeConfiguration<VerificationTokenReadModel>
{
    public void Configure(EntityTypeBuilder<VerificationTokenReadModel> builder)
    {
        builder.HasKey(a => a.VerificationTokenId);

        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.Temporary)
            .IsRequired()
            .HasColumnType("varchar(255)");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.CreatedDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ");

        builder.Property(a => a.ExpiredDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ");
    }
}
