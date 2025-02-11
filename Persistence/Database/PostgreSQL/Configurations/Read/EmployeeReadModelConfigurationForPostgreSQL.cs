using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converter;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class EmployeeReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<EmployeeReadModel>
{
    public void Configure(EntityTypeBuilder<EmployeeReadModel> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        builder.Property(x => x.EmployeeId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(x => x.EmployeeStatus)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(x => x.Role)
            .IsRequired()
            .HasColumnType("varchar(20)");
    }
}
