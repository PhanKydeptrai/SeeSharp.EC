using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class UserAuthenticationTokenConfigurationForPostgreSQL : IEntityTypeConfiguration<UserAuthenticationToken>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationToken> builder)
    {
        builder.HasKey(x => x.UserAuthenticationTokenId);

        builder.Property(x => x.UserAuthenticationTokenId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserAuthenticationTokenId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(x => x.Jti)
            .IsRequired()
            .HasColumnType("varchar(100)");
        
        builder.Property(x => x.ExpiredTime)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ");
        
        builder.Property(x => x.IsBlackList)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IsBlackList.FromBoolean(v))
            .HasColumnType("boolean");
        
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserId.FromGuid(value))
            .HasColumnType("uuid");
    }
}
