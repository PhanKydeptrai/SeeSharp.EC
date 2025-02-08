namespace Persistence.Database.PostgreSQL.ReadModels;

public class CategoryReadModel
{
    public string CategoryId { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string CategoryStatus { get; set; } = null!;

    public ICollection<ProductReadModel>? Products { get; set; } = new List<ProductReadModel>();
}
