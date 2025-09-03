namespace Application.DTOs.Feedbacks;

public record FeedbackResponse(
    Guid FeedbackId,
    string? Substance,
    float RatingScore,
    string? ImageUrl,
    Guid BillId,
    Guid CustomerId);
