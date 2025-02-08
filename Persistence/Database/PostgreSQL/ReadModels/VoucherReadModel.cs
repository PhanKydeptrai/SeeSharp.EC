namespace Persistence.Database.PostgreSQL.ReadModels;

public class VoucherReadModel
{
    public string VoucherId { get; set; } = null!;

    public string VoucherName { get; set; } = null!;

    public string VoucherCode { get; set; } = null!;

    public string VoucherType { get; set; } = null!;

    public int PercentageDiscount { get; set; }

    public decimal MaximumDiscountAmount { get; set; }

    public decimal MinimumOrderAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime ExpiredDate { get; set; }

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public ICollection<CustomerVoucherReadModel> CustomerVouchers { get; set; } = new List<CustomerVoucherReadModel>();

    public ICollection<OrderTransactionReadModel> OrderTransactions { get; set; } = new List<OrderTransactionReadModel>();
}
