using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.ChangeOrderStatus;

public record ChangeOrderStatusCommand(Guid OrderId) : ICommand;
