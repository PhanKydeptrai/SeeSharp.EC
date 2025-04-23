using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Vouchers;

namespace Domain.IRepositories.Vouchers;

public interface IVoucherRepository
{
    Task AddVoucher(Voucher voucher);
    Task<Voucher?> GetVoucherById(VoucherId voucherId);
    Task<Voucher?> GetVoucherByCode(VoucherCode voucherCode);
    Task<CustomerVoucher?> GetCustomerVoucherByVoucherCode(VoucherCode voucherCode, CustomerId customerId);
    Task<CustomerVoucher?> GetCustomerVoucherByVoucherId(VoucherId voucherId, CustomerId customerId);
    Task<bool> IsVoucherCodeExist(VoucherCode voucherCode);
    Task<bool> IsVoucherNameExist(VoucherName voucherName);
    Task UpdateVoucher(Voucher voucher);
    Task DeleteVoucher(VoucherId voucherId);
} 