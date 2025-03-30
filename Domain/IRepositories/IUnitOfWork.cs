using System.Data;

namespace Domain.IRepositories;

public interface IUnitOfWork
{
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    Task<IDbTransaction> BeginPostgreSQLTransaction();
}
