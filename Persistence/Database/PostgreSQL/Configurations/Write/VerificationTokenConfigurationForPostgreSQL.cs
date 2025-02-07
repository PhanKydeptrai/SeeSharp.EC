using NextSharp.Domain.Entities.VerificationTokenEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class VerificationTokenConfigurationForPostgreSQL : IEntityTypeConfiguration<VerificationToken>
{
    public void Configure(EntityTypeBuilder<VerificationToken> builder)
    {
        builder.HasKey(x => x.VerificationTokenId);
        builder.Property(a => a.VerificationTokenId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
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
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");
    }
}
