using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.AddProductToOrderForGuest;

public record AddProductToOrderForGuestCommand(
    Guid ProductVariantId,
    Guid GuestId,
    int Quantity) : ICommand;

public record AddProductToOrderForGuestRequest(Guid ProductVariantId, int Quantity);