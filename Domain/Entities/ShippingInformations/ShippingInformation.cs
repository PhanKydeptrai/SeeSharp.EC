using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Domain.Entities.ShippingInformations;
//NOTE: Create factory method
public sealed class ShippingInformation
{
    public ShippingInformationId ShippingInformationId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public FullName FullName { get; private set; } = FullName.Empty;
    public PhoneNumber PhoneNumber { get; private set; } = PhoneNumber.Empty;
    public IsDefault IsDefault { get; private set; } = null!;
    public SpecificAddress SpecificAddress { get; private set; } = SpecificAddress.Empty;
    public Province Province { get; private set; } = Province.Empty;
    public District District { get; private set; } = District.Empty;
    public Ward Ward { get; private set; } = Ward.Empty;
    //* Foreign key
    public Customer? Customer { get; set; } = null!;
    public ICollection<Bill>? Bills { get; set; } = null!;
}
