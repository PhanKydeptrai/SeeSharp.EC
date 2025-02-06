using Domain.Entities.Customers;
using Domain.Entities.Vouchers;

namespace Domain.Entities.CustomerVouchers;
//NOTE: Create factory method
public sealed class CustomerVoucher
{
    public CustomerVoucherId CustomerVoucherId { get; private set; } = null!;
    public VoucherId VoucherId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public CustomerVoucherQuantity Quantity { get; set; } = null!;

    //Foreign key
    public Voucher? Voucher { get; set; } = null!;
    public Customer? Customer { get; set; } = null!;
}
