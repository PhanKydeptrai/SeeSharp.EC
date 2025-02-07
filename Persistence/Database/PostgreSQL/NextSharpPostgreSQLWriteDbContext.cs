using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.PostgreSQL;

public sealed class NextSharpPostgreSQLWriteDbContext : DbContext
{
    public NextSharpPostgreSQLWriteDbContext(DbContextOptions<NextSharpPostgreSQLWriteDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NextSharpPostgreSQLWriteDbContext).Assembly,
            WriteConfigurationsFilter);

        base.OnModelCreating(modelBuilder);
    }

    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Database.PostgreSQL.Configurations.Write") ?? false;
}
