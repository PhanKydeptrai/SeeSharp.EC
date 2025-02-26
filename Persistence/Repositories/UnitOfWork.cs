using System.Data;
using System.Threading.Tasks;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly NextSharpMySQLWriteDbContext _nextSharpMySQLWriteDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _nextSharpPostgreSQLWriteDbContext;
    public UnitOfWork(
        NextSharpMySQLWriteDbContext nextSharpMySQLDbContext, 
        NextSharpPostgreSQLWriteDbContext nextSharpPostgreSQLWriteDbContext)
    {
        _nextSharpMySQLWriteDbContext = nextSharpMySQLDbContext;
        _nextSharpPostgreSQLWriteDbContext = nextSharpPostgreSQLWriteDbContext;
    }

    public async Task<IDbTransaction> BeginMySQLTransaction()
    {
        var transaction = await _nextSharpMySQLWriteDbContext.Database.BeginTransactionAsync();
        return transaction.GetDbTransaction();
    }

    public async Task<IDbTransaction> BeginPostgreSQLTransaction()
    {
        var transaction = await _nextSharpPostgreSQLWriteDbContext.Database.BeginTransactionAsync();
        return transaction.GetDbTransaction();
    }

    public async Task<int> SaveToMySQL(CancellationToken cancellationToken = default)
    {
        return await _nextSharpMySQLWriteDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveToPostgreSQL(CancellationToken cancellationToken = default)
    {
        return await _nextSharpPostgreSQLWriteDbContext.SaveChangesAsync(cancellationToken);
    }
}
