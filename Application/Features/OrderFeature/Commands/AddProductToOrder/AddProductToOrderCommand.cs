using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.AddProductToOrder;

public record AddProductToOrderCommand(
    Guid ProductVariantId,
    Guid CustomerId,
    int Quantity) : ICommand;

public record AddProductToOrderRequest(
    Guid ProductVariantId,
    int Quantity
);