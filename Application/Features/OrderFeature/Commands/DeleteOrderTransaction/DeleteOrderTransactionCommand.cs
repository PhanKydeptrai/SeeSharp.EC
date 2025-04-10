using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.DeleteOrderTransaction;

public record DeleteOrderTransactionCommand(Guid CustomerId) : ICommand;