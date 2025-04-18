using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.VnPayReturnUrl;

public record VnPayReturnUrlCommand(Guid OrderTransactionId) : ICommand<Guid>;
