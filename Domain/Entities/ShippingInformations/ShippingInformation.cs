using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Domain.Entities.ShippingInformations;
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

    private ShippingInformation(
        ShippingInformationId shippingInformationId,
        CustomerId customerId,
        FullName fullName,
        PhoneNumber phoneNumber,
        IsDefault isDefault,
        SpecificAddress specificAddress,
        Province province,
        District district,
        Ward ward)
    {
        ShippingInformationId = shippingInformationId;
        CustomerId = customerId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        IsDefault = isDefault;
        SpecificAddress = specificAddress;
        Province = province;
        District = district;
        Ward = ward;
    }

    //Factory method
    public static ShippingInformation NewShippingInformation(
        CustomerId customerId,
        FullName fullName,
        PhoneNumber phoneNumber,
        IsDefault isDefault,
        SpecificAddress specificAddress,
        Province province,
        District district,
        Ward ward)
    {
        return new ShippingInformation(
            ShippingInformationId.New(),
            customerId,
            fullName,
            phoneNumber,
            isDefault,
            specificAddress,
            province,
            district,
            ward);
    }

}
