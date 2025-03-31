// using Application.Abstractions.EventBus;
// using Application.Abstractions.Messaging;
// using Application.IServices;
// using Domain.Entities.OrderDetails;
// using Domain.Entities.Orders;
// using Domain.IRepositories;
// using Domain.IRepositories.Orders;
// using Domain.OutboxMessages.Services;
// using Domain.Utilities.Errors;
// using SharedKernel;

// namespace Application.Features.OrderFeature.Commands.UpdateOrderDetail;

// internal sealed class UpdateOrderDetailCommandHandler : ICommandHandler<UpdateOrderDetailCommand>
// {
//     #region Dependencies
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IOrderRepository _orderRepository;
//     private readonly IProductQueryServices _productQueryServices;
//     public UpdateOrderDetailCommandHandler(
//         IUnitOfWork unitOfWork,
//         IOrderRepository orderRepository,
//         IProductQueryServices productQueryServices,
//         IOutBoxMessageServices outBoxMessageServices,
//         IEventBus eventBus)
//     {
//         _unitOfWork = unitOfWork;
//         _orderRepository = orderRepository;
//         _productQueryServices = productQueryServices;
//     }
//     #endregion

//     public async Task<Result> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
//     {
//         OrderDetailId orderDetailId = OrderDetailId.FromGuid(request.OrderDetailId);

//         var orderDetail = await _orderRepository.GetOrderDetailByIdFromPostgreSQL(orderDetailId);

//         if (orderDetail is null) return Result.Failure(OrderError.OrderDetailNotFound(orderDetailId));
//         //get product price
//         var productPrice = await _productQueryServices.GetAvailableProductPrice(orderDetail.ProductId);

        
//         var orderTotal = orderDetail.Order!.Total.Value - orderDetail.UnitPrice.Value; //remove old order detail price

//         // Update quantity and unit price of order detail
//         orderDetail.ReCaculateUnitPrice(
//             OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
//             productPrice!);

//         orderTotal += orderDetail.UnitPrice.Value; //add new order detail price
        
//         orderDetail.Order.ReplaceOrderTotal(OrderTotal.FromDecimal(orderTotal));

//         await _unitOfWork.SaveChangeAsync();      
//         return Result.Success();
//     }
// }
