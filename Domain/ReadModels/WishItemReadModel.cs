namespace Domain.Database.PostgreSQL.ReadModels;

public class WishItemReadModel
{
    public Guid WishItemId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public ProductReadModel ProductReadModel { get; set; } = null!;
}
