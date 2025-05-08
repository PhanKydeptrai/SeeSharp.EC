using Application.Abstractions.Messaging;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Bills;
using Domain.IRepositories.Orders;
using Domain.IRepositories.ShippingInformations;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.MakePaymentForGuest;

internal sealed class MakePaymentForGuestCommandHandler : ICommandHandler<MakePaymentForGuestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBillRepository _billRepository;
    private readonly IShippingInformationRepository _shippingInformationRepository;
    private readonly IOrderRepository _orderRepository;

    public MakePaymentForGuestCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IBillRepository billRepository,
        IShippingInformationRepository shippingInformationRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _billRepository = billRepository;
        _shippingInformationRepository = shippingInformationRepository;
    }

    public async Task<Result> Handle(MakePaymentForGuestCommand request, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.GuestId);
        var orderInformation = await _orderRepository.GetWaitingOrderByCustomerId(customerId);

        if (orderInformation is null) //Không có đơn hàng nào
        {
            return Result.Failure(
                new Error(
                    "Order not found",
                    "The order does not exist for the given customer.",
                    ErrorType.Problem));
        }

        //Tạo thông tin vận chuyển mới
        var shippingInformation = ShippingInformation.NewShippingInformation(
            customerId,
            FullName.FromString(request.FullName),
            PhoneNumber.FromString(request.PhoneNumber),
            IsDefault.False,
            SpecificAddress.FromString(request.SpecificAddress),
            Province.FromString(request.Province),
            District.FromString(request.District),
            Ward.FromString(request.Ward));

        //Tạo bill mới
        var bill = Bill.NewBill(
            orderInformation.OrderId,
            customerId,
            DateTime.UtcNow,
            PaymentMethod.None,
            BillPaymentStatus.Pending,
            shippingInformation.ShippingInformationId);

        //Tạo order transaction mới
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
            bill.BillId); //Cập nhật khi khách thanh toán thành công

        await _shippingInformationRepository.AddNewShippingInformation(shippingInformation);
        await _billRepository.AddBill(bill);
        await _orderRepository.AddNewOrderTransaction(orderTransaction);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
