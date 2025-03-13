using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands;

public record AddProductToOrderCommand(
    Guid ProductId,
    Guid CustomerId,
    int Quantity) : ICommand;

public record AddProductToOrderRequest(
    Guid ProductId,
    int Quantity
);