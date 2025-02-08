namespace Persistence.Database.PostgreSQL.ReadModels;

public class ShippingInformationReadModel
{
    public string ShippingInformationId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool IsDefault { get; set; }

    public string SpecificAddress { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public ICollection<BillReadModel> Bills { get; set; } = new List<BillReadModel>();

    public CustomerReadModel Customer { get; set; } = null!;
}
