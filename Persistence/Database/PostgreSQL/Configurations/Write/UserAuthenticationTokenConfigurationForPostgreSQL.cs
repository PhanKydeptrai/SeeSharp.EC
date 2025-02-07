using NextSharp.Domain.Entities.UserAuthenticationTokenEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class UserAuthenticationTokenConfigurationForPostgreSQL : IEntityTypeConfiguration<UserAuthenticationToken>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationToken> builder)
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
            .HasColumnType("boolean");
        
        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

    }
}
