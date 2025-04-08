using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;

public record MakePaymentForSubscriberRequest(string? voucherCode);
public record MakePaymentForSubscriberCommand(Guid CustomerId, string? voucherCode) : ICommand;
//TODO: Guid? CustomerId cho trường hợp khách hàng không đăng nhập
