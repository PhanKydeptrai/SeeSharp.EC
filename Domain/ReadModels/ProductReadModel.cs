using Domain.Entities.Products;

namespace Domain.Database.PostgreSQL.ReadModels;

public class ProductReadModel
{
    public Ulid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public decimal ProductPrice { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public Ulid CategoryId { get; set; }
    public CategoryReadModel CategoryReadModel { get; set; } = null!;
    public ICollection<OrderDetailReadModel> OrderDetailReadModels { get; set; } = new List<OrderDetailReadModel>();
    public ICollection<WishItemReadModel> WishItemReadModels { get; set; } = new List<WishItemReadModel>();
}
