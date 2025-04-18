using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.PayOrderWithCOD;

public record PayOrderWithCODCommand(Guid OrderId) : ICommand<Guid>;
