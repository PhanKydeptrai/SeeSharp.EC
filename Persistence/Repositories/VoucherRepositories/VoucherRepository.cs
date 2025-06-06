using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Vouchers;
using Domain.IRepositories.Vouchers;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.VoucherRepositories;

internal sealed class VoucherRepository : IVoucherRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _dbContext;

    public VoucherRepository(SeeSharpPostgreSQLWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerVoucher?> GetCustomerVoucherByVoucherCode(
        VoucherCode voucherCode, 
        CustomerId customerId)
    {
        return await _dbContext.CustomerVouchers.Include(v => v.Voucher)
            .FirstOrDefaultAsync(
                v => v.Voucher!.VoucherCode == voucherCode 
                && v.CustomerId == customerId
                // && v.Quantity.Value > 0
                && v.Voucher!.Status == Status.Active);
    }

    public async Task<CustomerVoucher?> GetCustomerVoucherByVoucherId(VoucherId voucherId, CustomerId customerId)
    {
        return await _dbContext.CustomerVouchers.FirstOrDefaultAsync(a => a.VoucherId == voucherId && a.CustomerId == customerId);
    }

    public async Task AddVoucher(Voucher voucher)
    {
        await _dbContext.Vouchers.AddAsync(voucher);
    }

    public async Task<Voucher?> GetVoucherById(VoucherId voucherId)
    {
        return await _dbContext.Vouchers
            .FirstOrDefaultAsync(v => v.VoucherId == voucherId);
    }

    public async Task<Voucher?> GetVoucherByCode(VoucherCode voucherCode)
    {
        return await _dbContext.Vouchers
            .FirstOrDefaultAsync(v => v.VoucherCode == voucherCode);
    }

    public async Task<bool> IsVoucherCodeExist(VoucherCode voucherCode)
    {
        return await _dbContext.Vouchers
            .AnyAsync(v => v.VoucherCode == voucherCode);
    }

    public async Task<bool> IsVoucherNameExist(VoucherName voucherName)
    {
        return await _dbContext.Vouchers
            .AnyAsync(v => v.VoucherName == voucherName);
    }

    public async Task UpdateVoucher(Voucher voucher)
    {
        _dbContext.Vouchers.Update(voucher);
    }

    public async Task DeleteVoucher(VoucherId voucherId)
    {
        var voucher = await GetVoucherById(voucherId);
        if (voucher != null)
        {
            _dbContext.Vouchers.Remove(voucher);
        }
    }

    public async Task AddCustomerVoucher(CustomerVoucher customerVoucher)
    {
        await _dbContext.CustomerVouchers.AddAsync(customerVoucher);
    }
} 