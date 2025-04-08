using System.Data;

namespace Domain.IRepositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbTransaction> BeginPostgreSQLTransaction();
}
