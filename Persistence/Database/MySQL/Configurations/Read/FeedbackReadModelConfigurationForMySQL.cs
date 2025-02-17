using Domain.Entities.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class FeedbackReadModelConfigurationForMySQL : IEntityTypeConfiguration<FeedbackReadModel>
{
    public void Configure(EntityTypeBuilder<FeedbackReadModel> builder)
    {
        builder.HasKey(x => x.FeedbackId);

        builder.Property(x => x.FeedbackId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.Substance)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.RatingScore)
            .IsRequired()
            .HasColumnType("float");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        //Một order có một feedback
        builder.HasOne(x => x.OrderReadModel)
            .WithOne(x => x.FeedbackReadModel)
            .HasForeignKey<Feedback>(x => x.OrderId);

    }
}
