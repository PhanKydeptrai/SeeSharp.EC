using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Write;

internal sealed class UserAuthenticationTokenConfigurationForMySQL : IEntityTypeConfiguration<UserAuthenticationToken>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationToken> builder)
    {
        builder.HasKey(a => a.UserAuthenticationTokenId);
        builder.Property(a => a.UserAuthenticationTokenId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserAuthenticationTokenId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");
        
        builder.Property(a => a.Jti)
            .IsRequired(false)
            .HasColumnType("varchar(36)");

        builder.Property(a => a.Value)
            .IsRequired()
            .HasColumnType("varchar(500)");

        // builder.Property(a => a.TokenType)
        //     .IsRequired()
        //     .HasColumnType("int");

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
                value => value.Value,
                value => UserId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");
    }
}
