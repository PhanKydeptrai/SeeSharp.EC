namespace Persistence.Database.PostgreSQL.ReadModels;

public class OrderReadModel
{
    public string OrderId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public decimal Total { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public string OrderTransactionId { get; set; } = null!;

    public virtual BillReadModel? Bill { get; set; }

    public virtual CustomerReadModel Customer { get; set; } = null!;

    public virtual FeedbackReadModel? Feedback { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderTransactionReadModel? OrderTransaction { get; set; }
}
