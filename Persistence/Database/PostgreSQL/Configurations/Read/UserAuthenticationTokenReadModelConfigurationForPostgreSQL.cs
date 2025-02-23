using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class UserAuthenticationTokenReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<UserAuthenticationTokenReadModel>
{
    public void Configure(EntityTypeBuilder<UserAuthenticationTokenReadModel> builder)
    {
        builder.HasKey(a => a.UserAuthenticationTokenId);
        builder.Property(a => a.UserAuthenticationTokenId)
            .IsRequired()
            
            .HasColumnType("uuid");

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
            
            .HasColumnType("uuid");

    }
}
