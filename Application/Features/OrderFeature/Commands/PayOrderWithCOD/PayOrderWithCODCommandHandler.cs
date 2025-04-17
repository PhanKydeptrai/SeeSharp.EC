using Application.Abstractions.Messaging;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.IRepositories.Vouchers;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.PayOrderWithCOD;

internal sealed class PayOrderWithCODCommandHandler : ICommandHandler<PayOrderWithCODCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PayOrderWithCODCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IVoucherRepository voucherRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _voucherRepository = voucherRepository;
    }

    public async Task<Result> Handle(PayOrderWithCODCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.OrderId);
        var order = await _orderRepository.GetOrderById(orderId);
        if (order is null)
        {
            return Result.Failure(OrderError.OrderNotFound(orderId));
        }

        if (order!.OrderTransaction is null)
        {
            return Result.Failure<string>(OrderError.TransactionNotCreated(order.CustomerId));
        }

        order.OrderTransaction!.ChangeTransactionStatus(TransactionStatus.Completed);
        order.ChangePaymentStatus(OrderPaymentStatus.Unpaid);
        order.ChangeOrderStatus(OrderStatus.New);
        order.Bill!.ChangeBillStatus(BillPaymentStatus.Unpaid, PaymentMethod.Cash);


        //Xử lý voucher
        if (order.OrderTransaction.IsVoucherUsed == IsVoucherUsed.Used)
        {
            var voucher = await _voucherRepository.GetCustomerVoucherByVoucherId(
                order.OrderTransaction.VoucherId!, 
                CustomerId.FromGuid(order.CustomerId));
            
            if (voucher is null)
            {
                throw new Exception($"Voucher with ID {order.OrderTransaction.VoucherId} not found.");
            }

            voucher.ChangeCustomerVoucherQuantity(CustomerVoucherQuantity.FromInt(voucher.Quantity.Value - 1));
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

}
