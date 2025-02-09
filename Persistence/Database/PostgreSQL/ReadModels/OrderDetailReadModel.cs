namespace Persistence.Database.PostgreSQL.ReadModels;

public partial class OrderDetailReadModel
{
    public string OrderDetailId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public OrderReadModel Order { get; set; } = null!;

    public ProductReadModel Product { get; set; } = null!;
}
