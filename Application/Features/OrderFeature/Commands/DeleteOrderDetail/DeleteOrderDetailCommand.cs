using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.DeleteOrderDetail;

public record DeleteOrderDetailCommand(Guid OrderDetailId) : ICommand;
