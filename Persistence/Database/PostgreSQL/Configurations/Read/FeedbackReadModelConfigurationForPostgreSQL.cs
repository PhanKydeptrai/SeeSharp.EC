using Domain.Entities.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class FeedbackReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<FeedbackReadModel>
{
    public void Configure(EntityTypeBuilder<FeedbackReadModel> builder)
    {
        builder.HasKey(x => x.FeedbackId);

        builder.Property(x => x.FeedbackId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

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
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        //Một order có một feedback
        builder.HasOne(x => x.OrderReadModel)
            .WithOne(x => x.FeedbackReadModel)
            .HasForeignKey<Feedback>(x => x.OrderId);

    }
}
