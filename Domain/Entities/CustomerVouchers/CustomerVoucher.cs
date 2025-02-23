using Domain.Entities.Customers;
using Domain.Entities.Vouchers;

namespace Domain.Entities.CustomerVouchers;
public sealed class CustomerVoucher
{
    public CustomerVoucherId CustomerVoucherId { get; private set; } = null!;
    public VoucherId VoucherId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public CustomerVoucherQuantity Quantity { get; set; } = null!;

    //Foreign key
    public Voucher? Voucher { get; set; } = null!;
    public Customer? Customer { get; set; } = null!;

    private CustomerVoucher(
        CustomerVoucherId customerVoucherId,
        VoucherId voucherId,
        CustomerId customerId,
        CustomerVoucherQuantity quantity)
    {
        CustomerVoucherId = customerVoucherId;
        VoucherId = voucherId;
        CustomerId = customerId;
        Quantity = quantity;
    }

    public static CustomerVoucher NewCustomerVoucher(
        VoucherId voucherId,
        CustomerId customerId,
        CustomerVoucherQuantity quantity)
    {
        return new CustomerVoucher(
            CustomerVoucherId.New(),
            voucherId,
            customerId,
            quantity);
    }
}
