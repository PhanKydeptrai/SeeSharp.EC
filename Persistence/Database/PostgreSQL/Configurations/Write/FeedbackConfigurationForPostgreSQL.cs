using NextSharp.Domain.Entities.FeedbackEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class FeedbackConfigurationForPostgreSQL : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
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
        builder.HasOne(x => x.Order)
            .WithOne(x => x.Feedback)
            .HasForeignKey<Feedback>(x => x.OrderId);

    }
}
