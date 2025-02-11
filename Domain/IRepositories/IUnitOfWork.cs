namespace Domain.IRepositories;

public interface IUnitOfWork
{
    Task<int> Commit(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
