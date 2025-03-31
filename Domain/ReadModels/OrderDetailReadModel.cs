using Domain.ReadModels;

namespace Domain.Database.PostgreSQL.ReadModels;

public sealed class OrderDetailReadModel
{
    public Ulid OrderDetailId { get; set; }
    public Ulid OrderId { get; set; }
    public Ulid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public OrderReadModel OrderReadModel { get; set; } = null!;
    public ProductVariantReadModel ProductVariantReadModel { get; set; } = null!;
}
