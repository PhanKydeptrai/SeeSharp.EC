using Domain.Entities.Customers;
using Domain.Entities.Feedbacks;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class FeedbackConfigurationForPostgreSQL : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(x => x.FeedbackId);

        builder.Property(x => x.FeedbackId)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => FeedbackId.FromString(v))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.Substance)
            .IsRequired(false)
            .HasConversion(
                v => v!.Value.ToString(),
                v => Substance.FromString(v))
            .HasColumnType("varchar(255)");

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.RatingScore)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => RatingScore.FromFloat(v))
            .HasColumnType("float");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => OrderId.FromString(v))
            .HasColumnType("varchar(26)");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => CustomerId.FromString(v))
            .HasColumnType("varchar(26)");

        //Một order có một feedback
        builder.HasOne(x => x.Order)
            .WithOne(x => x.Feedback)
            .HasForeignKey<Feedback>(x => x.OrderId);

    }
}
