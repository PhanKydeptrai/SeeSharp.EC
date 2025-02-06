using Domain.Entities.Customers;
using Domain.Entities.Feedbacks;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configuration;

internal sealed class FeedbackConfigurationForMySQL : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(x => x.FeedbackId);

        builder.Property(x => x.FeedbackId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new FeedbackId(Ulid.Parse(v)))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.Substance)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value.ToString(),
                v => Substance.NewSubstance(v)) //!FIXME
            .HasColumnType("varchar(255)");

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.RatingScore)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => RatingScore.NewRatingScore(v)) //!FIXME
            .HasColumnType("float");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new OrderId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new CustomerId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        //Một order có một feedback
        builder.HasOne(x => x.Order)
            .WithOne(x => x.Feedback)
            .HasForeignKey<Feedback>(x => x.OrderId);

    }
}
