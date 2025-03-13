using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.AddProductToOrder;

public record AddProductToOrderCommand(
    Guid ProductId,
    Guid CustomerId,
    int Quantity) : ICommand;

public record AddProductToOrderRequest(
    Guid ProductId,
    int Quantity
);