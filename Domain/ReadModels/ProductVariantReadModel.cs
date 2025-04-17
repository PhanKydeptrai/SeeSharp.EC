using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.ProductVariants;

namespace Domain.ReadModels;

public class ProductVariantReadModel
{
    public Ulid ProductVariantId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public decimal ProductVariantPrice { get; set; }
    public string ColorCode { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Description { get; set; } = string.Empty;
    public Ulid ProductId { get; set; }
    public ProductVariantStatus ProductVariantStatus { get; set; }
    public bool IsBaseVariant { get; set; }
    public ProductReadModel? ProductReadModel { get; set; } = null!;
    public ICollection<WishItemReadModel> WishItemReadModels { get; set; } = new List<WishItemReadModel>();
    public ICollection<OrderDetailReadModel> OrderDetailReadModels { get; set; } = new List<OrderDetailReadModel>();
}
