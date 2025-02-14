using Domain.IRepositories;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly NextSharpMySQLDbContext _nextSharpMySQLDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _nextSharpPostgreSQLWriteDbContext;
    public UnitOfWork(
        NextSharpMySQLDbContext nextSharpMySQLDbContext, 
        NextSharpPostgreSQLWriteDbContext nextSharpPostgreSQLWriteDbContext)
    {
        _nextSharpMySQLDbContext = nextSharpMySQLDbContext;
        _nextSharpPostgreSQLWriteDbContext = nextSharpPostgreSQLWriteDbContext;
    }

    public async Task<int> Commit(CancellationToken cancellationToken = default)
    {
        return await _nextSharpMySQLDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _nextSharpPostgreSQLWriteDbContext.SaveChangesAsync(cancellationToken);
    }
}
