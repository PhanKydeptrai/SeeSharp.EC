using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.PayOrderWithVnPay;

public record PayOrderWithVnPayCommand(Guid orderId) : ICommand<string>;