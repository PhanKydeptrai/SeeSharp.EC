namespace Domain.Database.PostgreSQL.ReadModels;

public class OrderTransactionReadModel
{
    public Ulid OrderTransactionId { get; set; }

    public string? PayerName { get; set; }

    public string? PayerEmail { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public bool IsVoucherUsed { get; set; }

    public Ulid? VoucherId { get; set; }

    public Ulid OrderId { get; set; }

    public Ulid? BillId { get; set; }

    public BillReadModel? BillReadModel { get; set; }

    public OrderReadModel OrderReadModel { get; set; } = null!;

    public VoucherReadModel? VoucherReadModel { get; set; }
}
