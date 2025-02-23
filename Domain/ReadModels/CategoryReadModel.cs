namespace Domain.Database.PostgreSQL.ReadModels;

public class CategoryReadModel
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string CategoryStatus { get; set; } = string.Empty;
    public ICollection<ProductReadModel>? ProductReadModels { get; set; }
}
