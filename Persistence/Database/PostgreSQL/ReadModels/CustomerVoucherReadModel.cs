namespace Persistence.Database.PostgreSQL.ReadModels;

public class CustomerVoucherReadModel
{
    public string CustomerVoucherId { get; set; } = null!;

    public string VoucherId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public int Quantity { get; set; }

    public CustomerReadModel Customer { get; set; } = null!;

    public VoucherReadModel Voucher { get; set; } = null!;
}
