using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;

public record MakePaymentForSubscriberRequest(
    string? voucherCode, 
    Guid? ShippingInformationId,
    string FullName,
    string PhoneNumber,
    string Email,
    string Province,
    string District,
    string Ward, 
    string SpecificAddress);
public record MakePaymentForSubscriberCommand(
    Guid CustomerId, 
    string? voucherCode,
    string FullName,
    string PhoneNumber,
    string Email,
    string Province,
    string District,
    string Ward, 
    string SpecificAddress) : ICommand;