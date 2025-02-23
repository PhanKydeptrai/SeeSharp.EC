namespace Domain.Database.PostgreSQL.ReadModels;

public partial class OrderDetailReadModel
{
    public Guid OrderDetailId { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public OrderReadModel OrderReadModel { get; set; } = null!;

    public ProductReadModel ProductReadModel { get; set; } = null!;
}
