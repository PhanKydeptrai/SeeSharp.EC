namespace Persistence.Database.PostgreSQL.ReadModels;

public class ProductReadModel
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public decimal ProductPrice { get; set; }

    public string ProductStatus { get; set; } = null!;

    public string CategoryId { get; set; } = null!;

    public CategoryReadModel Category { get; set; } = null!;

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public ICollection<WishItemReadModel> WishItems { get; set; } = new List<WishItemReadModel>();
}
