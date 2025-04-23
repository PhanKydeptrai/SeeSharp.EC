using Application.Abstractions.Messaging;

namespace Application.Features.ShippingInformationFeature.Commands.CreateShippingInformation;

public record CreateShippingInformationCommand(
    Guid CustomerId,
    string FullName,
    string PhoneNumber,
    string Province,
    string SpecificAddress

) : ICommand;
