namespace Domain.Database.PostgreSQL.ReadModels;

public partial class OrderDetailReadModel
{
    public Ulid OrderDetailId { get; set; }

    public Ulid OrderId { get; set; }

    public Ulid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public OrderReadModel OrderReadModel { get; set; } = null!;

    public ProductReadModel ProductReadModel { get; set; } = null!;
}
