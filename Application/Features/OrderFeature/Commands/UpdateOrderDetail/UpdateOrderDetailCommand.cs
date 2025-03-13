using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.UpdateOrderDetail;

public record UpdateOrderDetailCommand(Guid OrderDetailId, int Quantity) : ICommand;
public record UpdateOrderDetailRequest(int Quantity);    
