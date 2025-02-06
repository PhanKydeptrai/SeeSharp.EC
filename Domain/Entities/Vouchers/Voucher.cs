namespace Domain.Entities.Vouchers;

//NOTE: Create factory method
public sealed class Voucher
{
    public VoucherId VoucherId { get; set; } = null!;
    public VoucherName VoucherName { get; set; } = VoucherName.Empty;
    public VoucherCode VoucherCode { get; set; } = VoucherCode.Empty;
    public VoucherType VoucherType { get; set; }
    public int PercentageDiscount { get; set; }
    public decimal MaximumDiscountAmount { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public string Description { get; set; } = null!;
    public Status Status { get; set; }

    //Foreign key
    // public ICollection<CustomerVoucher> CustomerVouchers { get; set; } = null!;
    // public ICollection<OrderTransaction> OrderTransactions { get; set; } = null!;
}

