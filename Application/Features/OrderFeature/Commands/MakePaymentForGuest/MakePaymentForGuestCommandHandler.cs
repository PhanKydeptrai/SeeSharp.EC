using Application.Abstractions.Messaging;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.MakePaymentForGuest;

internal sealed class MakePaymentForGuestCommandHandler : ICommandHandler<MakePaymentForGuestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public MakePaymentForGuestCommandHandler(
        IUnitOfWork unitOfWork, 
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(MakePaymentForGuestCommand request, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.GuestId);
        var orderInformation = await _orderRepository.GetOrderByCustomerId(customerId);
        
        if (orderInformation is null) //Không có đơn hàng nào
        {
            return Result.Failure(
                new Error(
                    "Order not found",
                    "The order does not exist for the given customer.",
                    ErrorType.Problem));
        }

        var orderTransaction = OrderTransaction.NewOrderTransaction(
                PayerName.Empty,
                Email.Empty,
                AmountOfOrderTransaction.FromDecimal(orderInformation.Total.Value),
                DescriptionOfOrderTransaction.FromString("Payment for order"),
                PaymentMethod.None,
                IsVoucherUsed.NotUsed,
                TransactionStatus.Pending,
                null,
                orderInformation.OrderId,
                null //Cập nhật khi khách thanh toán thành công
            );

            await _orderRepository.AddNewOrderTransaction(orderTransaction);
            await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
