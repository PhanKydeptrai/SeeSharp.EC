using System.Data;

namespace Domain.IRepositories;

public interface IUnitOfWork
{
    Task<int> SaveToMySQL(CancellationToken cancellationToken = default);
    Task<int> SaveToPostgreSQL(CancellationToken cancellationToken = default);
    Task<IDbTransaction> BeginMySQLTransaction();
    Task<IDbTransaction> BeginPostgreSQLTransaction();
}
