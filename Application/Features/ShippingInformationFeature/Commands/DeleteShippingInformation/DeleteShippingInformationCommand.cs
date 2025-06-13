using Application.Abstractions.Messaging;

namespace Application.Features.ShippingInformationFeature.Commands.DeleteShippingInformation;

public record DeleteShippingInformationCommand(Guid ShippingInformationId) : ICommand;
