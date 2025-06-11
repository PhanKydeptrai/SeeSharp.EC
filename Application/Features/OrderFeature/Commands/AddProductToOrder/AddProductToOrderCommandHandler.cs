using Application.Abstractions.Messaging;
using Application.Features.OrderFeature.Commands.AddProductToOrder;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands;
//NOTE: Refactor
internal sealed class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrderCommand>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductQueryServices _productQueryServices;
    private readonly IOrderRepository _orderRepository;
    public AddProductToOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IProductQueryServices productQueryServices)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _productQueryServices = productQueryServices;
    }

    public async Task<Result> Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {

        var order = await _orderRepository.GetWaitingOrderByCustomerId(CustomerId.FromGuid(request.CustomerId));

        ProductVariantId productVariantId = ProductVariantId.FromGuid(request.ProductVariantId);
        var productPrice = await _productQueryServices.GetAvailableProductPrice(productVariantId);

        if (productPrice is null) return Result.Failure(ProductError.VariantNotFound(productVariantId));

        if (order is not null) //* Order is exist
        {
            var orderDetail = await _orderRepository.CheckProductAvailabilityInOrder(order.OrderId, productVariantId);
            if (orderDetail is not null) //* Order detail is exist
            {
                var orderTotal = order.Total.Value - orderDetail.UnitPrice.Value;

                // Update quantity and unit price of order detail
                orderDetail.UpdateQuantityAndProductPriceAfterAddMoreProduct(
                    OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                    productPrice!);

                // Update order total 
                orderTotal = orderTotal + orderDetail.UnitPrice.Value;
                order.ReplaceOrderTotal(OrderTotal.FromDecimal(orderTotal));                
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            else
            {
                //* Order detail is not exist
                var newOrderDetail = OrderDetail.NewOrderDetail(
                   order.OrderId,
                   ProductVariantId.FromGuid(request.ProductVariantId),
                   OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                   productPrice!);

                order.AddNewValueToOrderTotal(newOrderDetail.UnitPrice);
                await _orderRepository.AddNewOrderDetail(newOrderDetail);
                
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
        }
        else
        {
            //* Order is not exist
            var newOrder = Order.NewOrder(
                CustomerId.FromGuid(request.CustomerId),
                OrderTotal.FromDecimal(productPrice!.Value * request.Quantity),
                OrderPaymentStatus.Waiting,
                OrderStatus.Waiting);

            var newOrderDetail = OrderDetail.NewOrderDetail(
                newOrder.OrderId,
                ProductVariantId.FromGuid(request.ProductVariantId),
                OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                productPrice!);

            await _orderRepository.AddNewOrder(newOrder);

            await _orderRepository.AddNewOrderDetail(newOrderDetail);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }

}
