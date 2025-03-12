using System.Reflection;
using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.IServices;
using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands;

internal sealed class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrderCommand>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IProductQueryServices _productQueryServices;
    private readonly IOrderQueryServices _orderQueryServices;
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    public AddProductToOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IEventBus eventBus,
        IOrderRepository orderRepository,
        IOrderQueryServices orderQueryServices,
        IProductQueryServices productQueryServices)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _eventBus = eventBus;
        _orderRepository = orderRepository;
        _orderQueryServices = orderQueryServices;
        _productQueryServices = productQueryServices;
    }

    public async Task<Result> Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {
        
        var (orderId, order, orderTransaction) = await GetOrCreateOrderAsync(request);

        ProductId productId = ProductId.FromGuid(request.ProductId);
        var productPrice = await _productQueryServices.GetAvailableProductPrice(
            ProductId.FromGuid(request.ProductId));
        
        if(productPrice is null)
        {
            return Result.Failure(ProductError.NotFound(productId));
        }

        if(orderId is not null)
        {
            //Xử lý thêm sản phẩm vào order

            var orderDetail = OrderDetail.NewOrderDetail(
                orderId, 
                ProductId.FromGuid(request.ProductId),
                OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                productPrice!);
            

        }
        
        //Add order and order transaction to database
        //Create orderdetail
        return Result.Success();
    }

    private async Task<(OrderId? orderId, Order? order, OrderTransaction? orderTransaction)> GetOrCreateOrderAsync(AddProductToOrderCommand request)
    {
        var orderId = await _orderQueryServices.CheckOrderAvailability(CustomerId.FromGuid(request.CustomerId));
        if (orderId is not null) return (orderId, null, null);
        

        //1. Create new order
        var order = Order.NewOrder(
            CustomerId.FromGuid(request.CustomerId), 
            OrderTotal.FromDecimal(0), 
            OrderPaymentStatus.Waiting, 
            OrderStatus.Waiting);

        //2. Create new order transaction
        var orderTransaction = OrderTransaction.NewOrderTransaction(
            PayerName.Empty, 
            Email.Empty, 
            AmountOfOrderTransaction.FromDecimal(0), 
            DescriptionOfOrderTransaction.Empty, 
            PaymentMethod.Cash, 
            IsVoucherUsed.NotUsed, 
            null,
            order.OrderId,
            null);

        return (null, order, orderTransaction);
    }
}
