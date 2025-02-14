using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class VerificationTokenConfigurationForPostgreSQL : IEntityTypeConfiguration<VerificationToken>
{
    public void Configure(EntityTypeBuilder<VerificationToken> builder)
    {
        builder.HasKey(x => x.VerificationTokenId);
        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => VerificationTokenId.FromGuid(value))
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
            .HasConversion(
                value => value.Value.ToGuid(),
                value => UserId.FromGuid(value))
            .HasColumnType("uuid");
    }
}
