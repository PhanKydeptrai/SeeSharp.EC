using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class VerificationTokenReadModelConfigurationForMySQL : IEntityTypeConfiguration<VerificationTokenReadModel>
{
    public void Configure(EntityTypeBuilder<VerificationTokenReadModel> builder)
    {
        builder.HasKey(x => x.VerificationTokenId);
        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

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
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");
    }
}
