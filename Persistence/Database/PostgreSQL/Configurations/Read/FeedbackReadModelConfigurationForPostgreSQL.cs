using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class FeedbackReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<FeedbackReadModel>
{
    public void Configure(EntityTypeBuilder<FeedbackReadModel> builder)
    {
        builder.HasKey(x => x.FeedbackId);

        builder.Property(x => x.FeedbackId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.Substance)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.RatingScore)
            .IsRequired()
            .HasColumnType("float");
        
        builder.Property(x => x.IsPrivate)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");
    }
}
