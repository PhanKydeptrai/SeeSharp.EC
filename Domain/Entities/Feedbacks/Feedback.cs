using Domain.Entities.Bills;
using Domain.Entities.Customers;

namespace Domain.Entities.Feedbacks;
public sealed class Feedback
{
    public FeedbackId FeedbackId { get; private set; } = null!;
    public Substance? Substance { get; private set; } = null!;
    public RatingScore RatingScore { get; private set; } = null!;
    public IsPrivate IsPrivate { get; private set; } = null!;
    public string? ImageUrl { get; private set; } = string.Empty;
    public BillId BillId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    //* Foreign Key
    public Customer? Customer { get; set; }
    public Bill? Bill { get; set; }


    private Feedback(
        FeedbackId feedbackId,
        Substance substance,
        RatingScore ratingScore,
        IsPrivate isPrivate,
        string imageUrl,
        BillId billId,
        CustomerId customerId)
    {
        FeedbackId = feedbackId;
        Substance = substance;
        RatingScore = ratingScore;
        IsPrivate = isPrivate;
        ImageUrl = imageUrl;
        BillId = billId;
        CustomerId = customerId;
    }

    public static Feedback NewFeedback(
        Substance? substance,
        RatingScore ratingScore,
        string? imageUrl,
        IsPrivate isPrivate,
        BillId billId,
        CustomerId customerId)
    {
        return new Feedback(
            FeedbackId.New(),
            substance ?? Substance.Empty,
            ratingScore,
            isPrivate,
            imageUrl ?? string.Empty,
            billId,
            customerId);
    }
    
    public void UpdatFeedback(
        Substance substance,
        RatingScore ratingScore,
        string? imageUrl)
    {
        Substance = substance;
        RatingScore = ratingScore;
        ImageUrl = imageUrl ?? string.Empty;
    }
}
