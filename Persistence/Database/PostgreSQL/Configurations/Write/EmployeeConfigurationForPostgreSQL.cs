using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Domain.Entities.EmployeeEntity;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class EmployeeConfigurationForPostgreSQL : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
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
            // .HasConversion<EnumToStringConverter<EmployeeStatus>>()
                .HasColumnType("varchar(20)");

        builder.Property(x => x.Role)
            .IsRequired()
            // .HasConversion<EnumToStringConverter<Role>>()
                .HasColumnType("varchar(20)");
    }
}
