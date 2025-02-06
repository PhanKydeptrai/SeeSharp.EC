namespace Domain.Entities.Vouchers;

//NOTE: Create factory method
public sealed class Voucher
{
    public VoucherId VoucherId { get; private set; } = null!;
    public VoucherName VoucherName { get; private set; } = VoucherName.Empty;
    public VoucherCode VoucherCode { get; private set; } = VoucherCode.Empty;
    public VoucherType VoucherType { get; private set; }
    public PercentageDiscount PercentageDiscount { get; private set; } = null!;
    public MaximumDiscountAmount MaximumDiscountAmount { get; private set; } = null!;
    public MinimumOrderAmount MinimumOrderAmount { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime ExpiredDate { get; private set; }
    public VoucherDescription Description { get; private set; } = null!;
    public Status Status { get; private set; }

    //Foreign key
    // public ICollection<CustomerVoucher> CustomerVouchers { get; set; } = null!;
    // public ICollection<OrderTransaction> OrderTransactions { get; set; } = null!;
}
