using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.MakePaymentForGuest;

public record MakePaymentForGuestRequest(
    string Email,
    string FullName,
    string PhoneNumber,
    string Province,
    string District,
    string Ward,
    string SpecificAddress);

public record MakePaymentForGuestCommand(
    Guid GuestId,
    string Email,
    string FullName,
    string PhoneNumber,
    string Province,
    string District,
    string Ward,
    string SpecificAddress) : ICommand;