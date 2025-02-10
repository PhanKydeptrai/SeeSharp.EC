namespace Domain.Database.PostgreSQL.ReadModels;

public class OrderReadModel
{
    public Ulid OrderId { get; set; }

    public Ulid CustomerId { get; set; }

    public decimal Total { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public Ulid OrderTransactionId { get; set; }

    public BillReadModel? BillReadModel { get; set; }

    public CustomerReadModel CustomerReadModel { get; set; } = null!;

    public FeedbackReadModel? FeedbackReadModel { get; set; }

    public ICollection<OrderDetailReadModel> OrderDetailReadModels { get; set; } = new List<OrderDetailReadModel>();

    public OrderTransactionReadModel? OrderTransactionReadModel { get; set; }
}
