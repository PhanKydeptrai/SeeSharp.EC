namespace Domain.Database.PostgreSQL.ReadModels;

public class CustomerVoucherReadModel
{
    public Ulid CustomerVoucherId { get; set; }
    public Ulid VoucherId { get; set; } 
    public Ulid CustomerId { get; set; }
    public int Quantity { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public VoucherReadModel VoucherReadModel { get; set; } = null!;
}
