namespace Domain.Database.PostgreSQL.ReadModels;

public class FeedbackReadModel
{
    public Ulid FeedbackId { get; set; }
    public string? Substance { get; set; }
    public float RatingScore { get; set; }
    public string? ImageUrl { get; set; }
    public Ulid BillId { get; set; }
    public Ulid CustomerId { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public OrderReadModel OrderReadModel { get; set; } = null!;
}
