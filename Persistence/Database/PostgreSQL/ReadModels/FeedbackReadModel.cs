namespace Persistence.Database.PostgreSQL.ReadModels;

public class FeedbackReadModel
{
    public Ulid FeedbackId { get; set; }
    public string? Substance { get; set; }
    public double RatingScore { get; set; }
    public string? ImageUrl { get; set; }
    public Ulid OrderId { get; set; }
    public Ulid CustomerId { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public OrderReadModel OrderReadModel { get; set; } = null!;
}
