using Application.Abstractions.Messaging;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;
using Domain.IRepositories;
using Domain.IRepositories.Bills;
using Domain.IRepositories.Orders;
using Domain.IRepositories.ShippingInformations;
using Domain.IRepositories.Vouchers;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;
/// <summary>
/// Tạo order transaction cho đơn hàng của khách hàng, khách sẽ dùng voucher ở đây
/// </summary>
internal sealed class MakePaymentForSubscriberCommandHandler : ICommandHandler<MakePaymentForSubscriberCommand>
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IShippingInformationRepository _shippingInformationRepository;
    private readonly IBillRepository _billRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    public MakePaymentForSubscriberCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IVoucherRepository voucherRepository,
        IShippingInformationRepository shippingInformationRepository,
        IBillRepository billRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _voucherRepository = voucherRepository;
        _shippingInformationRepository = shippingInformationRepository;
        _billRepository = billRepository;
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

        if (request.voucherCode is not null) // Có sử dụng voucher
        {

            var customerVoucher = await _voucherRepository.GetCustomerVoucherByVoucherCode(
                VoucherCode.FromString(request.voucherCode),
                customerId);

            if (customerVoucher is null) //Nếu => voucher hết hạn || Khách không có || Vouncher Chưa bắt đầu
            {
                return Result.Failure(
                    new Error(
                        "Voucher not found",
                        "The voucher does not exist for the given customer.",
                        ErrorType.Problem));
            }

            if (orderInformation.Total.Value < customerVoucher.Voucher!.MinimumOrderAmount.Value) //Nếu đơn hàng nhỏ hơn giá trị tối thiểu của voucher
            {
                return Result.Failure(
                    new Error(
                        "MinimumOrderAmount.NotMet",
                        "The order amount does not meet the minimum order amount for the voucher.",
                        ErrorType.Problem));
            }

            //Kiểm tra loại voucher nào
            if (customerVoucher.Voucher!.VoucherType == VoucherType.Direct) //#1
            {

                //Tính thành tiền
                var total = orderInformation.Total.Value - customerVoucher.Voucher.MaximumDiscountAmount.Value;

                if (total < 0) total = 0; //Nếu thành tiền nhỏ hơn 0 thì thành tiền = 0

                if (request.ShippingInformationId is null) //Không có địa chỉ sẵn
                {
                    //Tạo thông tin vận chuyển mới
                    var shippingInformation = ShippingInformation.NewShippingInformation(
                        ShippingInformationId.New(),
                        customerId,
                        FullName.FromString(request.FullName!),
                        PhoneNumber.FromString(request.PhoneNumber!),
                        IsDefault.False,
                        SpecificAddress.FromString(request.SpecificAddress!),
                        Province.FromString(request.Province!),
                        District.FromString(request.District!),
                        Ward.FromString(request.Ward!));

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
                        AmountOfOrderTransaction.FromDecimal(total),
                        DescriptionOfOrderTransaction.FromString("Payment for order"),
                        PaymentMethod.None,
                        IsVoucherUsed.Used,
                        TransactionStatus.Pending,
                        customerVoucher.VoucherId,
                        orderInformation.OrderId,
                        bill.BillId);

                    await _shippingInformationRepository.AddNewShippingInformation(shippingInformation);
                    await _billRepository.AddBill(bill);
                    await _orderRepository.AddNewOrderTransaction(orderTransaction);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId.Value);

                    // Kiểm tra xem shippingInformationId có tồn tại không
                    if (!await _shippingInformationRepository.IsExistedShippingInformation(shippingInformationId))
                    {
                        return Result.Failure(
                            new Error(
                                "ShippingInformation not found",
                                "The shipping information does not exist.",
                                ErrorType.Problem));
                    }

                    //Tạo bill mới
                    var bill = Bill.NewBill(
                        orderInformation.OrderId,
                        customerId,
                        DateTime.UtcNow,
                        PaymentMethod.None,
                        BillPaymentStatus.Pending,
                        shippingInformationId);

                    //Tạo order transaction mới
                    var orderTransaction = OrderTransaction.NewOrderTransaction(
                        PayerName.Empty,
                        Email.Empty,
                        AmountOfOrderTransaction.FromDecimal(total),
                        DescriptionOfOrderTransaction.FromString("Payment for order"),
                        PaymentMethod.None,
                        IsVoucherUsed.Used,
                        TransactionStatus.Pending,
                        customerVoucher.VoucherId,
                        orderInformation.OrderId,
                        bill.BillId);

                    await _billRepository.AddBill(bill);
                    await _orderRepository.AddNewOrderTransaction(orderTransaction);
                    await _unitOfWork.SaveChangesAsync();

                }
            }
            else //Nếu là voucher phần trăm //#2
            {

                //Tính thành tiền
                var total = orderInformation.Total.Value - (orderInformation.Total.Value * customerVoucher.Voucher.PercentageDiscount.Value / 100);

                if (total < 0) total = 0; //Nếu thành tiền nhỏ hơn 0 thì thành tiền = 0

                if (request.ShippingInformationId is null) //Không có địa chỉ sẵn
                {
                    //Tạo thông tin vận chuyển mới
                    var shippingInformation = ShippingInformation.NewShippingInformation(
                        ShippingInformationId.New(),
                        customerId,
                        FullName.FromString(request.FullName!),
                        PhoneNumber.FromString(request.PhoneNumber!),
                        IsDefault.False,
                        SpecificAddress.FromString(request.SpecificAddress!),
                        Province.FromString(request.Province!),
                        District.FromString(request.District!),
                        Ward.FromString(request.Ward!));

                    //Tạo bill mới
                    var bill = Bill.NewBill(
                        orderInformation.OrderId,
                        customerId,
                        DateTime.UtcNow,
                        PaymentMethod.None,
                        BillPaymentStatus.Pending,
                        shippingInformation.ShippingInformationId);

                    var orderTransaction = OrderTransaction.NewOrderTransaction(
                        PayerName.Empty,
                        Email.Empty,
                        AmountOfOrderTransaction.FromDecimal(total),
                        DescriptionOfOrderTransaction.FromString("Payment for order"),
                        PaymentMethod.None,
                        IsVoucherUsed.Used,
                        TransactionStatus.Pending,
                        customerVoucher.VoucherId,
                        orderInformation.OrderId,
                        bill.BillId);

                    await _shippingInformationRepository.AddNewShippingInformation(shippingInformation);
                    await _billRepository.AddBill(bill);
                    await _orderRepository.AddNewOrderTransaction(orderTransaction);
                    await _unitOfWork.SaveChangesAsync();

                }
                else
                {
                    var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId.Value);

                    // Kiểm tra xem shippingInformationId có tồn tại không
                    if (!await _shippingInformationRepository.IsExistedShippingInformation(shippingInformationId))
                    {
                        return Result.Failure(
                            new Error(
                                "ShippingInformation not found",
                                "The shipping information does not exist.",
                                ErrorType.Problem));
                    }

                    //Tạo bill mới
                    var bill = Bill.NewBill(
                        orderInformation.OrderId,
                        customerId,
                        DateTime.UtcNow,
                        PaymentMethod.None,
                        BillPaymentStatus.Pending,
                        shippingInformationId);

                    var orderTransaction = OrderTransaction.NewOrderTransaction(
                        PayerName.Empty,
                        Email.Empty,
                        AmountOfOrderTransaction.FromDecimal(total),
                        DescriptionOfOrderTransaction.FromString("Payment for order"),
                        PaymentMethod.None,
                        IsVoucherUsed.Used,
                        TransactionStatus.Pending,
                        customerVoucher.VoucherId,
                        orderInformation.OrderId,
                        bill.BillId);

                    await _billRepository.AddBill(bill);
                    await _orderRepository.AddNewOrderTransaction(orderTransaction);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
        else
        {
            if (request.ShippingInformationId is null) //Không có địa chỉ sẵn //#3
            {
                //Tạo thông tin vận chuyển mới
                var shippingInformation = ShippingInformation.NewShippingInformation(
                    ShippingInformationId.New(),
                    customerId,
                    FullName.FromString(request.FullName!),
                    PhoneNumber.FromString(request.PhoneNumber!),
                    IsDefault.False,
                    SpecificAddress.FromString(request.SpecificAddress!),
                    Province.FromString(request.Province!),
                    District.FromString(request.District!),
                    Ward.FromString(request.Ward!));

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
                    null, //
                    orderInformation.OrderId,
                    bill.BillId);

                await _shippingInformationRepository.AddNewShippingInformation(shippingInformation);
                await _billRepository.AddBill(bill);
                await _orderRepository.AddNewOrderTransaction(orderTransaction);
                await _unitOfWork.SaveChangesAsync();
            }
            else //Lấy thông tin vận chuyển
            {
                var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId.Value);

                // Kiểm tra xem shippingInformationId có tồn tại không
                if (!await _shippingInformationRepository.IsExistedShippingInformation(shippingInformationId))
                {
                    return Result.Failure(
                        new Error(
                            "ShippingInformation not found",
                            "The shipping information does not exist.",
                            ErrorType.Problem));
                }

                //Tạo bill mới
                var bill = Bill.NewBill(
                    orderInformation.OrderId,
                    customerId,
                    DateTime.UtcNow,
                    PaymentMethod.None,
                    BillPaymentStatus.Pending,
                    shippingInformationId);

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
                    bill.BillId);

                await _billRepository.AddBill(bill);
                await _orderRepository.AddNewOrderTransaction(orderTransaction);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        return Result.Success();
    }
}
