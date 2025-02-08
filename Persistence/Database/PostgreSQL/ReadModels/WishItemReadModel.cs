namespace Persistence.Database.PostgreSQL.ReadModels;

public class WishItemReadModel
{
    public string WishItemId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public CustomerReadModel Customer { get; set; } = null!;

    public ProductReadModel Product { get; set; } = null!;
}
