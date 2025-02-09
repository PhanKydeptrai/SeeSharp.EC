namespace Persistence.Database.PostgreSQL.ReadModels;

public class OrderReadModel
{
    public string OrderId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public decimal Total { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public string OrderTransactionId { get; set; } = null!;

    public BillReadModel? Bill { get; set; }

    public CustomerReadModel Customer { get; set; } = null!;

    public FeedbackReadModel? Feedback { get; set; }

    public ICollection<OrderDetailReadModel> OrderDetails { get; set; } = new List<OrderDetailReadModel>();

    public OrderTransactionReadModel? OrderTransaction { get; set; }
}
