using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands;

public record AddProductToOrderCommand(
    Guid ProductId, 
    int Quantity) : ICommand;