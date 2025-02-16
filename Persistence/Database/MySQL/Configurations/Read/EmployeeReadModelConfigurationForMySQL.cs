using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.MySQL.Configurations.Read;

internal sealed class EmployeeReadModelConfigurationForMySQL : IEntityTypeConfiguration<EmployeeReadModel>
{
    public void Configure(EntityTypeBuilder<EmployeeReadModel> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        builder.Property(x => x.EmployeeId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(value => value.ToGuid(), value => new Ulid(value))
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.EmployeeStatus)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(x => x.Role)
            .IsRequired()
            .HasColumnType("varchar(20)");
    }
}
