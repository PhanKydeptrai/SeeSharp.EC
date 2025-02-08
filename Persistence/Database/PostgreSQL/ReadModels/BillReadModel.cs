namespace Persistence.Database.PostgreSQL.ReadModels;

public class BillReadModel
{
    public string BillId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string ShippingInformationId { get; set; } = null!;

    public virtual CustomerReadModel Customer { get; set; } = null!;

    public virtual OrderReadModel Order { get; set; } = null!;

    public virtual ICollection<OrderTransactionReadModel> OrderTransactions { get; set; } = new List<OrderTransactionReadModel>();

    public virtual ShippingInformationReadModel ShippingInformation { get; set; } = null!;
}
