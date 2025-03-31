// using Application.Abstractions.Messaging;
// using Application.DTOs.Order;
// using Application.IServices;
// using Domain.Entities.Orders;
// using Domain.Utilities.Errors;
// using SharedKernel;

// namespace Application.Features.OrderFeature.Queries.GetOrderByOrderId;

// internal sealed class GetOrderByOrderIdQueryHandler : IQueryHandler<GetOrderByOrderIdQuery, OrderResponse>
// {
//     private readonly IOrderQueryServices _orderQueryServices;
//     public GetOrderByOrderIdQueryHandler(IOrderQueryServices orderQueryServices)
//     {
//         _orderQueryServices = orderQueryServices;
//     }

//     public async Task<Result<OrderResponse>> Handle(GetOrderByOrderIdQuery request, CancellationToken cancellationToken)
//     {
//         OrderId orderId = OrderId.FromGuid(request.OrderId);
//         var order = await _orderQueryServices.GetOrderById(orderId);
//         if(order is null) return Result.Failure<OrderResponse>(OrderError.OrderNotFound(orderId));
//         return Result.Success(order);
//     }
// }
