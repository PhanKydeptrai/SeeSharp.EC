using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.PostgreSQL;

public sealed class NextSharpPostgreSQLReadDbContext : DbContext
{
    public NextSharpPostgreSQLReadDbContext(DbContextOptions<NextSharpPostgreSQLReadDbContext> options) : base(options) 
    {
        this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NextSharpPostgreSQLReadDbContext).Assembly,
            WriteConfigurationsFilter);

        base.OnModelCreating(modelBuilder);
    }
    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Database.PostgreSQL.Configurations.Read") ?? false;
}
