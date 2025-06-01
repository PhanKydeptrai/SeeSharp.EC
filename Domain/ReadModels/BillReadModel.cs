using Domain.Entities.Bills;

namespace Domain.Database.PostgreSQL.ReadModels;

public class BillReadModel
{
    public Ulid BillId { get; set; } 
    public Ulid OrderId { get; set; }
    public Ulid CustomerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Ulid ShippingInformationId { get; set; }
    public bool IsRated { get; set; }
    public BillPaymentStatus BillPaymentStatus { get; set; }
    public CustomerReadModel Customer { get; set; } = null!;
    public FeedbackReadModel? Feedback { get; set; } = null!;
    public OrderReadModel Order { get; set; } = null!;
    public ShippingInformationReadModel ShippingInformationReadModel { get; set; } = new ShippingInformationReadModel();
}
