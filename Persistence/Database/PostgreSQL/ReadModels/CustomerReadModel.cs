namespace Persistence.Database.PostgreSQL.ReadModels;

public class CustomerReadModel
{
    public string CustomerId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string CustomerStatus { get; set; } = null!;

    public string CustomerType { get; set; } = null!;

    public virtual ICollection<BillReadModel> Bills { get; set; } = new List<BillReadModel>();

    public virtual ICollection<CustomerVoucherReadModel> CustomerVouchers { get; set; } = new List<CustomerVoucherReadModel>();

    public virtual ICollection<FeedbackReadModel> Feedbacks { get; set; } = new List<FeedbackReadModel>();

    public virtual ICollection<OrderReadModel> Orders { get; set; } = new List<OrderReadModel>();

    public virtual ICollection<ShippingInformationReadModel> ShippingInformations { get; set; } = new List<ShippingInformationReadModel>();

    public virtual UserReadModel User { get; set; } = null!;

    public virtual ICollection<WishItemReadModel> WishItems { get; set; } = new List<WishItemReadModel>();
}
