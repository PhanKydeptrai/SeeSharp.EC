using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class VerificationTokenReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<VerificationToken>
{
    public void Configure(EntityTypeBuilder<VerificationToken> builder)
    {
        builder.HasKey(x => x.VerificationTokenId);
        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new VerificationTokenId(Ulid.Parse(v))
            )
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
            .HasConversion(
                v => v.Value.ToString(),
                v => new UserId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");
    }
}
