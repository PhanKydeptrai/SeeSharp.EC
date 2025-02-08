namespace Persistence.Database.PostgreSQL.ReadModels;

public class FeedbackReadModel
{
    public string FeedbackId { get; set; } = null!;

    public string? Substance { get; set; }

    public double RatingScore { get; set; }

    public string? ImageUrl { get; set; }

    public string OrderId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public virtual CustomerReadModel Customer { get; set; } = null!;

    public virtual OrderReadModel Order { get; set; } = null!;
}
