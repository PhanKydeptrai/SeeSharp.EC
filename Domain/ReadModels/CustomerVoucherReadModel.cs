namespace Domain.Database.PostgreSQL.ReadModels;

public class CustomerVoucherReadModel
{
    public Guid CustomerVoucherId { get; set; }
    public Guid VoucherId { get; set; } 
    public Guid CustomerId { get; set; }
    public int Quantity { get; set; }
    public CustomerReadModel CustomerReadModel { get; set; } = null!;
    public VoucherReadModel VoucherReadModel { get; set; } = null!;
}
