namespace Domain.Database.PostgreSQL.ReadModels;

public class FeedbackReadModel
{
    public Guid FeedbackId { get; set; }
    public string? Substance { get; set; }
    public double RatingScore { get; set; }
    public string? ImageUrl { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public OrderReadModel OrderReadModel { get; set; } = null!;
}
