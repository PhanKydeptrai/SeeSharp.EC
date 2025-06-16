using Domain.Entities.Bills;
using Domain.ReadModels;

namespace Domain.Database.PostgreSQL.ReadModels;

public class BillReadModel
{
    public Ulid BillId { get; set; }
    public Ulid OrderId { get; set; }
    public Ulid CustomerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Ulid ShippingInformationId { get; set; }
    public bool IsRated { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string SpecificAddress { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string District { get; set; } = null!;
    public string Ward { get; set; } = null!;
    public BillPaymentStatus BillPaymentStatus { get; set; }
    public CustomerReadModel Customer { get; set; } = null!;
    public FeedbackReadModel? Feedback { get; set; } = null!;
    public OrderReadModel Order { get; set; } = null!;
    public OrderTransactionReadModel? OrderTransaction { get; set; } = null!;
    public ICollection<BillDetailReadModel> BillDetails { get; set; } = new List<BillDetailReadModel>();
}
