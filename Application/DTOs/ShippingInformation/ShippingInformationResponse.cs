namespace Application.DTOs.ShippingInformation;

public record ShippingInformationResponse(
    Guid ShippingInformationId,
    Guid CustomerId,
    string FullName,
    string PhoneNumber,
    bool IsDefault,
    string SpecificAddress,
    string Province,
    string District,
    string Ward);