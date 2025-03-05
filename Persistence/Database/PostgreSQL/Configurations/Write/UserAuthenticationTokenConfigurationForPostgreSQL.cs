using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class UserAuthenticationTokenConfigurationForPostgreSQL : IEntityTypeConfiguration<UserAuthenticationToken>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationToken> builder)
    {
        
        builder.HasKey(a => a.UserAuthenticationTokenId);
        
        builder.Property(a => a.UserAuthenticationTokenId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserAuthenticationTokenId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.Value)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(a => a.TokenType)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.ExpiredTime)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.IsBlackList)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IsBlackList.FromBoolean(v)
            )
            .HasColumnType("boolean");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserId.FromGuid(value))
            .HasColumnType("uuid");

    }
}
