using Application.Abstractions.Messaging;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;
/// <summary>
/// Tạo order transaction cho đơn hàng của khách hàng, khách sẽ dùng voucher ở đây
/// </summary>
internal sealed class MakePaymentForSubscriberCommandHandler : ICommandHandler<MakePaymentForSubscriberCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    public MakePaymentForSubscriberCommandHandler(
        IUnitOfWork unitOfWork, 
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(MakePaymentForSubscriberCommand request, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.CustomerId);
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
            IsVoucherUsed.NotUsed, //TODO: Sẽ có cơ chế để sử dụng voucher trong tương lai
            TransactionStatus.Pending,
            null, //
            orderInformation.OrderId,
            null //Cập nhật khi khách thanh toán thành công
        );
    
        await _orderRepository.AddNewOrderTransaction(orderTransaction);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
