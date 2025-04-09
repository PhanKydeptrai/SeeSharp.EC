using Application.Abstractions.Messaging;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.ProductVariants;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.Orders;
using Domain.IRepositories.Users;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.AddProductToOrderForGuest;

internal sealed class AddProductToOrderForGuestCommandHandler : ICommandHandler<AddProductToOrderForGuestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductQueryServices _productQueryServices;
    private readonly IOrderRepository _orderRepository;

    public AddProductToOrderForGuestCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IProductQueryServices productQueryServices,
        IUserRepository userRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productQueryServices = productQueryServices;
        _userRepository = userRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Result> Handle(AddProductToOrderForGuestCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByCustomerId(CustomerId.FromGuid(request.GuestId));
        
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
            // Create new Normal customer, new order and order detail

            var user = User.NewUser(
                null,
                UserName.Empty, 
                Email.Empty, 
                PhoneNumber.Empty, 
                PasswordHash.Empty, 
                null, 
                string.Empty);

            var customer = Customer.FromExisting(
                CustomerId.FromGuid(request.GuestId), 
                user.UserId, 
                CustomerType.Normal);

            var newOrder = Order.NewOrder(
                CustomerId.FromGuid(request.GuestId),
                OrderTotal.FromDecimal(productPrice!.Value * request.Quantity),
                OrderPaymentStatus.Waiting,
                OrderStatus.Waiting);

            var newOrderDetail = OrderDetail.NewOrderDetail(
                newOrder.OrderId,
                ProductVariantId.FromGuid(request.ProductVariantId),
                OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                productPrice!);

            await _userRepository.AddUser(user);
            
            await _customerRepository.AddCustomer(customer);

            await _orderRepository.AddNewOrder(newOrder);

            await _orderRepository.AddNewOrderDetail(newOrderDetail);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
