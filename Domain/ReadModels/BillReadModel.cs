namespace Domain.Database.PostgreSQL.ReadModels;

public class BillReadModel
{
    public Guid BillId { get; set; } 
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public Guid ShippingInformationId { get; set; }
    public CustomerReadModel Customer { get; set; } = null!;
    public OrderReadModel Order { get; set; } = null!;
    public ICollection<OrderTransactionReadModel> OrderTransactions { get; set; } = new List<OrderTransactionReadModel>();
    public ShippingInformationReadModel ShippingInformation { get; set; } = new ShippingInformationReadModel();
}
