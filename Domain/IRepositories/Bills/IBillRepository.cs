using Domain.Entities.Bills;

namespace Domain.IRepositories.Bills;

public interface IBillRepository
{
    Task AddBill(Bill bill);
}
