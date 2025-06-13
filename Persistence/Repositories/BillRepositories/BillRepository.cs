using Domain.Entities.BillDetails;
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

    public async Task AddBillDetail(BillDetail billDetail)
    {
        await _dbContext.BillDetails.AddAsync(billDetail);
    }

    public async Task AddBillDetail(List<BillDetail> billDetail)
    {
        await _dbContext.BillDetails.AddRangeAsync(billDetail);
    }

    public void RemoveBill(Bill bill)
    {
        _dbContext.Bills.Remove(bill);
    }
}
