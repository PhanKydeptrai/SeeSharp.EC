using Domain.Entities.Employees;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MySQL.Configurations.Write;

internal sealed class EmployeeConfigurationForMySQL : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        builder.Property(x => x.EmployeeId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => EmployeeId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserId.FromGuid(value)
            )
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.EmployeeStatus)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(x => x.Role)
            .IsRequired()
            .HasColumnType("int");
    }
}
