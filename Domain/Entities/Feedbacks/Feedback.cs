using Domain.Entities.Customers;
using Domain.Entities.Orders;

namespace Domain.Entities.Feedbacks;
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

    private Feedback(
        FeedbackId feedbackId,
        Substance substance,
        RatingScore ratingScore,
        string imageUrl,
        OrderId orderId,
        CustomerId customerId)
    {
        FeedbackId = feedbackId;
        Substance = substance;
        RatingScore = ratingScore;
        ImageUrl = imageUrl;
        OrderId = orderId;
        CustomerId = customerId;
    }

    public static Feedback NewFeedback(
        Substance? substance,
        RatingScore ratingScore,
        string? imageUrl,
        OrderId orderId,
        CustomerId customerId)
    {
        return new Feedback(
            FeedbackId.New(),
            substance ?? Substance.Empty,
            ratingScore,
            imageUrl ?? string.Empty,
            orderId,
            customerId);
    }
}
