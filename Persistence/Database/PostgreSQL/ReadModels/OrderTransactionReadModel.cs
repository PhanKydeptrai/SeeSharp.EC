namespace Persistence.Database.PostgreSQL.ReadModels;

public class OrderTransactionReadModel
{
    public string OrderTransactionId { get; set; } = null!;

    public string? PayerName { get; set; }

    public string? PayerEmail { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public bool IsVoucherUsed { get; set; }

    public string? VoucherId { get; set; }

    public string OrderId { get; set; } = null!;

    public string? BillId { get; set; }

    public virtual BillReadModel? Bill { get; set; }

    public virtual OrderReadModel Order { get; set; } = null!;

    public virtual VoucherReadModel? Voucher { get; set; }
}
