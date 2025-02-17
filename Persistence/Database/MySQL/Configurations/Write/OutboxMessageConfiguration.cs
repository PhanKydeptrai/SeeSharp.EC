using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace Persistence.Database.MySQL.Configurations.Write;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(a => a.Type)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (OutboxMessageStatus)Enum.Parse(typeof(OutboxMessageStatus), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.Error)
            .IsRequired(false)
            .HasColumnType("TEXT");

        builder.Property(a => a.Content)
            .IsRequired()
            .HasColumnType("JSON");

        builder.Property(a => a.OccurredOnUtc)
            .IsRequired()
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.ProcessedOnUtc)
            .IsRequired(false)
            .HasColumnType("TIMESTAMP");
    }
}
