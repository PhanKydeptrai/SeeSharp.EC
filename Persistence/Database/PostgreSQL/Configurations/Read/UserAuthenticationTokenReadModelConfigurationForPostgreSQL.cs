using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Persistence.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class UserAuthenticationTokenReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<UserAuthenticationTokenReadModel>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationTokenReadModel> builder)
    {
        builder.HasKey(a => a.UserAuthenticationTokenId);
        builder.Property(a => a.UserAuthenticationTokenId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.Value)
            .IsRequired()
            .HasColumnType("varchar(256)");

        builder.Property(a => a.TokenType)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(a => a.ExpiredTime)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.IsBlackList)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

    }
}
