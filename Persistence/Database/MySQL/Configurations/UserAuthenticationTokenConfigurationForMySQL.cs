using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations;

internal sealed class UserAuthenticationTokenConfigurationForMySQL : IEntityTypeConfiguration<UserAuthenticationToken>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationToken> builder)
    {
        builder.HasKey(a => a.UserAuthenticationTokenId);
        builder.Property(a => a.UserAuthenticationTokenId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new UserAuthenticationTokenId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        builder.Property(a => a.Value)
            .IsRequired()
            .HasColumnType("varchar(256)");

        builder.Property(a => a.TokenType)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (TokenType)Enum.Parse(typeof(TokenType), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.ExpiredTime)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.IsBlackList)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IsBlackList.FromBoolean(v)
            )
            .HasColumnType("tinyint(1)");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new UserId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");
    }
}
