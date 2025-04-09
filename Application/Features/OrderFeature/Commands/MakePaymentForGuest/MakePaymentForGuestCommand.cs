using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.MakePaymentForGuest;

public record MakePaymentForGuestCommand(Guid GuestId) : ICommand;