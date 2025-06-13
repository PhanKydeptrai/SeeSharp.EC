using Domain.Entities.BillDetails;
using Domain.Entities.Bills;

namespace Domain.IRepositories.Bills;

public interface IBillRepository
{
    /// <summary>
    /// Thêm mới bill
    /// </summary>
    /// <param name="bill"></param>
    /// <returns></returns>
    Task AddBill(Bill bill);

    /// <summary>
    /// Lấy bill theo Id
    /// </summary>
    /// <param name="bill"></param>
    void RemoveBill(Bill bill);

    /// <summary>
    /// Lấy bill theo Id
    /// </summary>
    /// <param name="billDetail"></param>
    /// <returns></returns>
    Task AddBillDetail(List<BillDetail> billDetail);
}
