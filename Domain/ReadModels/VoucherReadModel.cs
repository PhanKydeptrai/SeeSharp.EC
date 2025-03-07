using Domain.Entities.Vouchers;

namespace Domain.Database.PostgreSQL.ReadModels;

public class VoucherReadModel
{
    public Ulid VoucherId { get; set; }
    public string VoucherName { get; set; } = null!;
    public string VoucherCode { get; set; } = null!;
    public VoucherType VoucherType { get; set; }
    public int PercentageDiscount { get; set; }
    public decimal MaximumDiscountAmount { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public string Description { get; set; } = null!;
    public Status Status { get; set; }
    public ICollection<CustomerVoucherReadModel> CustomerVoucherReadModels { get; set; } = new List<CustomerVoucherReadModel>();
    public ICollection<OrderTransactionReadModel> OrderTransactionReadModels { get; set; } = new List<OrderTransactionReadModel>();
}
