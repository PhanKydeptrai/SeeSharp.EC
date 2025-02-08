namespace Persistence.Database.PostgreSQL.ReadModels;

public class CustomerVoucherReadModel
{
    public string CustomerVoucherId { get; set; } = null!;

    public string VoucherId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual CustomerReadModel Customer { get; set; } = null!;

    public virtual VoucherReadModel Voucher { get; set; } = null!;
}
