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
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(a => a.Type)
            .IsRequired()
            .HasColumnType("TEXT");

        builder.Property(a => a.Status)
            .IsRequired()
            .HasColumnType("int");

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
