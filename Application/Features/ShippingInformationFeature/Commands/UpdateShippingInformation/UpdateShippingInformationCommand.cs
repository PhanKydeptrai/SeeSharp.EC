using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Features.ShippingInformationFeature.Commands.UpdateShippingInformation;

public record UpdateShippingInformationCommand(
    Guid CustomerId,
    Guid ShippingInformationId,
    string FullName,
    string PhoneNumber,
    string Province,
    string District,
    bool IsDefault,
    string Ward,
    string SpecificAddress) : ICommand;
