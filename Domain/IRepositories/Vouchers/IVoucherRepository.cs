using Domain.Entities.Vouchers;

namespace Domain.IRepositories.Vouchers;

public interface IVoucherRepository
{
    Task AddVoucher(Voucher voucher);
    Task<Voucher?> GetVoucherById(VoucherId voucherId);
    Task<Voucher?> GetVoucherByCode(VoucherCode voucherCode);
    Task<bool> IsVoucherCodeExist(VoucherCode voucherCode);
    Task<bool> IsVoucherNameExist(VoucherName voucherName);
    Task UpdateVoucher(Voucher voucher);
    Task DeleteVoucher(VoucherId voucherId);
} 