using Domain.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Database.PostgreSQL;
using System.Data;

namespace Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly SeeSharpPostgreSQLWriteDbContext _nextSharpPostgreSQLWriteDbContext;
    public UnitOfWork(SeeSharpPostgreSQLWriteDbContext nextSharpPostgreSQLWriteDbContext)
    {
        _nextSharpPostgreSQLWriteDbContext = nextSharpPostgreSQLWriteDbContext;
    }

    public async Task<IDbTransaction> BeginPostgreSQLTransaction()
    {
        var transaction = await _nextSharpPostgreSQLWriteDbContext.Database.BeginTransactionAsync();
        return transaction.GetDbTransaction();
    }

    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await _nextSharpPostgreSQLWriteDbContext.SaveChangesAsync(cancellationToken);
    }
}
