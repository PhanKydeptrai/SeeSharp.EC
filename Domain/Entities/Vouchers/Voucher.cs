using Domain.Entities.CustomerVouchers;
using Domain.Entities.OrderTransactions;

namespace Domain.Entities.Vouchers;
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
    public ICollection<CustomerVoucher> CustomerVouchers { get; set; } = null!;
    public ICollection<OrderTransaction> OrderTransactions { get; set; } = null!;

    private Voucher(
        VoucherId voucherId,
        VoucherName voucherName,
        VoucherCode voucherCode,
        VoucherType voucherType,
        PercentageDiscount percentageDiscount,
        MaximumDiscountAmount maximumDiscountAmount,
        MinimumOrderAmount minimumOrderAmount,
        DateTime startDate,
        DateTime expiredDate,
        VoucherDescription description,
        Status status)
    {
        VoucherId = voucherId;
        VoucherName = voucherName;
        VoucherCode = voucherCode;
        VoucherType = voucherType;
        PercentageDiscount = percentageDiscount;
        MaximumDiscountAmount = maximumDiscountAmount;
        MinimumOrderAmount = minimumOrderAmount;
        StartDate = startDate;
        ExpiredDate = expiredDate;
        Description = description;
        Status = status;
    }

    //Factory method
    public static Voucher NewDirectDiscountVoucher(
        VoucherName voucherName,
        VoucherCode voucherCode,
        MaximumDiscountAmount maximumDiscountAmount,
        MinimumOrderAmount minimumOrderAmount,
        DateTime startDate,
        DateTime expiredDate,
        VoucherDescription voucherDescription)
    {
        if(startDate > expiredDate)
        {
            throw new ArgumentException(
                "Start date must be less than expired date", nameof(startDate));
        }
        
        return new Voucher(
            VoucherId.New(),
            voucherName,
            voucherCode,
            VoucherType.Direct,
            PercentageDiscount.FromInt(0),
            maximumDiscountAmount,
            minimumOrderAmount,
            startDate,
            expiredDate,
            voucherDescription,
            Status.Active);
    }

    public static Voucher NewPercentageDiscountVoucher(
        VoucherName voucherName,
        VoucherCode voucherCode,
        PercentageDiscount percentageDiscount,
        MaximumDiscountAmount maximumDiscountAmount,
        MinimumOrderAmount minimumOrderAmount,
        DateTime startDate,
        DateTime expiredDate,
        VoucherDescription voucherDescription)
    {
        if (startDate > expiredDate)
        {
            throw new ArgumentException(
                "Start date must be less than expired date", nameof(startDate));
        }

        return new Voucher(
            VoucherId.New(),
            voucherName,
            voucherCode,
            VoucherType.Percentage,
            percentageDiscount,
            maximumDiscountAmount,
            minimumOrderAmount,
            startDate,
            expiredDate,
            voucherDescription,
            Status.Active);
    }

}
