namespace Persistence.Database.PostgreSQL.ReadModels;

public class CustomerReadModel
{
    public Ulid CustomerId { get; set; } 
    public Ulid UserId { get; set; }
    public string CustomerStatus { get; set; } = null!;
    public string CustomerType { get; set; } = null!;
    public ICollection<BillReadModel> BillReadModels { get; set; } = new List<BillReadModel>();
    public ICollection<CustomerVoucherReadModel> CustomerVoucherReadModels { get; set; } = new List<CustomerVoucherReadModel>();
    public ICollection<FeedbackReadModel> FeedbackReadModels { get; set; } = new List<FeedbackReadModel>();
    public ICollection<OrderReadModel> OrderReadModels { get; set; } = new List<OrderReadModel>();
    public ICollection<ShippingInformationReadModel> ShippingInformations { get; set; } = new List<ShippingInformationReadModel>();
    public UserReadModel UserReadModel { get; set; } = null!;
    public ICollection<WishItemReadModel> WishItems { get; set; } = new List<WishItemReadModel>();
}
