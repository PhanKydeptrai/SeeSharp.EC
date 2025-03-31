using Domain.ReadModels;

namespace Domain.Database.PostgreSQL.ReadModels;

public class WishItemReadModel
{
    public Ulid WishItemId { get; set; }
    public Ulid CustomerId { get; set; }
    public Ulid ProductVariantId { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public ProductVariantReadModel ProductVariantReadModel { get; set; } = null!;
}
