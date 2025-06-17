using Application.Abstractions.Messaging;
using Domain.Entities.BillDetails;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Bills;
using Domain.IRepositories.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.MakePaymentForGuest;

internal sealed class MakePaymentForGuestCommandHandler : ICommandHandler<MakePaymentForGuestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBillRepository _billRepository;
    private readonly IOrderRepository _orderRepository;

    public MakePaymentForGuestCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IBillRepository billRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _billRepository = billRepository;
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

        //Tạo bill mới
        var bill = Bill.NewBill(
            orderInformation.OrderId,
            customerId,
            DateTime.UtcNow,
            BillPaymentStatus.Pending,
            FullName.FromString(request.FullName),
            PhoneNumber.FromString(request.PhoneNumber),
            Email.FromString(request.Email),
            SpecificAddress.FromString(request.SpecificAddress),
            Province.FromString(request.Province),
            District.FromString(request.District),
            Ward.FromString(request.Ward));

        //Tạo bill detail từ order
        var billDetails = orderInformation.OrderDetails!.Select(orderDetail =>
            BillDetail.Create(
                bill.BillId,
                orderDetail.ProductVariant!.Product!.ProductName,
                orderDetail.ProductVariant!.VariantName,
                orderDetail.ProductVariant!.ProductVariantPrice,
                BillDetailUnitPrice.FromDecimal(orderDetail.UnitPrice.Value),
                orderDetail.ProductVariant.ImageUrl ?? string.Empty,
                BillDetailQuantity.FromInt(orderDetail.Quantity.Value),
                orderDetail.ProductVariant.ColorCode,
                orderDetail.ProductVariant.Description)).ToList();

        //Tạo order transaction mới
        var orderTransaction = OrderTransaction.NewOrderTransaction(
            PayerName.FromString(request.FullName),
            Email.FromString(request.Email),
            AmountOfOrderTransaction.FromDecimal(orderInformation.Total.Value),
            DescriptionOfOrderTransaction.FromString("Payment for order"),
            PaymentMethod.None,
            IsVoucherUsed.NotUsed,
            TransactionStatus.Pending,
            null,
            orderInformation.OrderId,
            bill.BillId); //Cập nhật khi khách thanh toán thành công

        await _billRepository.AddBill(bill);
        await _billRepository.AddBillDetail(billDetails);
        await _orderRepository.AddNewOrderTransaction(orderTransaction);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
