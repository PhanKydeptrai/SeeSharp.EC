namespace Domain.Database.PostgreSQL.ReadModels;

public class WishItemReadModel
{
    public Ulid WishItemId { get; set; }
    public Ulid CustomerId { get; set; }
    public Ulid ProductId { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public ProductReadModel ProductReadModel { get; set; } = null!;
}
