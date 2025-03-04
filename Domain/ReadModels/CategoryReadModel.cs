using Domain.Entities.Categories;

namespace Domain.Database.PostgreSQL.ReadModels;

public class CategoryReadModel
{
    public Ulid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public CategoryStatus CategoryStatus { get; set; }
    public bool IsDefault { get; set; }
    public ICollection<ProductReadModel>? ProductReadModels { get; set; }
}
