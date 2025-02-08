namespace Persistence.Database.PostgreSQL.ReadModels;

public class CustomerReadModel
{
    public string CustomerId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string CustomerStatus { get; set; } = null!;

    public string CustomerType { get; set; } = null!;

    public ICollection<BillReadModel> Bills { get; set; } = new List<BillReadModel>();

    public ICollection<CustomerVoucherReadModel> CustomerVouchers { get; set; } = new List<CustomerVoucherReadModel>();

    public ICollection<FeedbackReadModel> Feedbacks { get; set; } = new List<FeedbackReadModel>();

    public ICollection<OrderReadModel> Orders { get; set; } = new List<OrderReadModel>();

    public ICollection<ShippingInformationReadModel> ShippingInformations { get; set; } = new List<ShippingInformationReadModel>();

    public UserReadModel User { get; set; } = null!;

    public ICollection<WishItemReadModel> WishItems { get; set; } = new List<WishItemReadModel>();
}
