using Domain.Entities.Customers;
using Domain.Entities.Orders;

namespace Domain.Entities.Feedbacks;
//NOTE: Create factory method
public sealed class Feedback
{
    public FeedbackId FeedbackId { get; private set; } = null!;
    public Substance? Substance { get; private set; } = null!;
    public RatingScore RatingScore { get; private set; } = null!;
    public string? ImageUrl { get; private set; } = string.Empty;
    public OrderId OrderId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    //* Foreign Key
    public Customer? Customer { get; set; }
    public Order? Order { get; set; }
}
