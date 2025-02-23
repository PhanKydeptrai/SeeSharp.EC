using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.MySQL.Configurations.Read;

internal sealed class EmployeeReadModelConfigurationForMySQL : IEntityTypeConfiguration<EmployeeReadModel>
{
    public void Configure(EntityTypeBuilder<EmployeeReadModel> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        builder.Property(x => x.EmployeeId)
            .IsRequired()
            
            .HasColumnType("char(36)")
            .HasDefaultValueSql("(UUID())");

        builder.Property(x => x.UserId)
            .IsRequired()
            
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
