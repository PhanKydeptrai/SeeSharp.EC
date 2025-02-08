namespace Persistence.Database.PostgreSQL.ReadModels;

public class BillReadModel
{
    public string BillId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string ShippingInformationId { get; set; } = null!;

    public CustomerReadModel Customer { get; set; } = null!;

    public OrderReadModel Order { get; set; } = null!;

    public ICollection<OrderTransactionReadModel> OrderTransactions { get; set; } = new List<OrderTransactionReadModel>();

    public ShippingInformationReadModel ShippingInformation { get; set; } = new ShippingInformationReadModel();
}
