using Application.Abstractions.Messaging;
using Application.Extentions;
using Domain.Entities.Orders;
using Domain.IRepositories.Orders;
using Domain.Utilities.Errors;
using Microsoft.Extensions.Configuration;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.PayOrderWithVnPay;

internal sealed class PayOrderWithVnPayCommandHandler : ICommandHandler<PayOrderWithVnPayCommand, string>
{
    private readonly IConfiguration _configuration;
    private readonly IOrderRepository _orderRepository;
    public PayOrderWithVnPayCommandHandler(
        IOrderRepository orderRepository,
        IConfiguration configuration)
    {
        _orderRepository = orderRepository;
        _configuration = configuration;
    }

    public async Task<Result<string>> Handle(PayOrderWithVnPayCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.orderId);
        var order = await _orderRepository.GetOrderById(orderId);
        if (order is null)
        {
            return Result.Failure<string>(OrderError.OrderNotFound(orderId));
        }

        if (order!.OrderTransaction is null)
        {
            return Result.Failure<string>(OrderError.TransactionNotCreated(order.CustomerId));
        }

        #region VnPay
        string paymentUrl = VnPayExtentions.GetVnPayUrl(
            _configuration["VNPAY:VNP_RETURNURL_ORDERS"]!,
            (int)order.OrderTransaction.Amount.Value,
            order.OrderTransaction.OrderTransactionId.ToString(),
            _configuration);
        #endregion

        return Result.Success(paymentUrl);
    }
}
