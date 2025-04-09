using Domain.Entities.Bills;
using Domain.IRepositories.Bills;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.BillRepositories;

internal sealed class BillRepository : IBillRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _dbContext;

    public BillRepository(SeeSharpPostgreSQLWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddBill(Bill bill)
    {
        await _dbContext.Bills.AddAsync(bill);
    }
}
