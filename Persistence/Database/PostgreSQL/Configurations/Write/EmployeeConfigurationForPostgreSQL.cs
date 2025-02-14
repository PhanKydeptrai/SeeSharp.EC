using Domain.Entities.Bills;
using Domain.Entities.Employees;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class EmployeeConfigurationForPostgreSQL : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        builder.Property(x => x.EmployeeId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => EmployeeId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(
                value => value.Value.ToGuid(),
                value => UserId.FromGuid(value))
            .HasColumnType("uuid");

        builder.Property(x => x.EmployeeStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EmployeeStatus)Enum.Parse(typeof(EmployeeStatus), v))
            .HasColumnType("varchar(20)");

        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v))
            .HasColumnType("varchar(20)");
    }
}
