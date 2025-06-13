using Domain.Entities.Bills;
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
                value => value.Value,
                value => FeedbackId.FromGuid(value))
            .HasColumnType("uuid");

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

        builder.Property(x => x.BillId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => BillId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => CustomerId.FromGuid(value))
            .HasColumnType("uuid");

        //Một order có một feedback
        builder.HasOne(x => x.Bill)
            .WithOne(x => x.Feedback)
            .HasForeignKey<Feedback>(x => x.BillId);

    }
}
