using Application.Abstractions.Messaging;
using Domain.Entities.Bills;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.IRepositories.Vouchers;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.VnPayReturnUrl;

internal sealed class VnPayReturnUrlCommandHandler : ICommandHandler<VnPayReturnUrlCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUnitOfWork _unitOfWork;
    public VnPayReturnUrlCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IVoucherRepository voucherRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _voucherRepository = voucherRepository;
    }

    public async Task<Result<Guid>> Handle(VnPayReturnUrlCommand request, CancellationToken cancellationToken)
    {
        var orderTransactionId = OrderTransactionId.FromGuid(request.OrderTransactionId);
        var orderTransaction = await _orderRepository.GetOrderTransactionById(orderTransactionId);
        if (orderTransaction is null)
        {
            return Result.Failure<Guid>(OrderError.TransactionNotFound(orderTransactionId));
        }

        orderTransaction.ChangeTransactionStatus(TransactionStatus.Completed);
        orderTransaction.Order!.ChangePaymentStatus(OrderPaymentStatus.Paid);
        orderTransaction.Order.ChangeOrderStatus(OrderStatus.New);
        orderTransaction.Order.Bill!.ChangeBillStatus(BillPaymentStatus.Paid, PaymentMethod.Vnpay);

        //Xử lý voucher
        if (orderTransaction.IsVoucherUsed == IsVoucherUsed.Used)
        {
            var voucher = await _voucherRepository.GetCustomerVoucherByVoucherId(
                orderTransaction.VoucherId!, 
                orderTransaction.Order.CustomerId);
            
            if (voucher is null)
            {
                throw new Exception($"Voucher with ID {orderTransaction.VoucherId} not found.");
            }

            voucher.ChangeCustomerVoucherQuantity(CustomerVoucherQuantity.FromInt(voucher.Quantity.Value - 1));
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(orderTransaction.Order.OrderId.Value);
        }

        await _unitOfWork.SaveChangesAsync();
        return Result.Success(orderTransaction.Order.OrderId.Value);
    }
}
