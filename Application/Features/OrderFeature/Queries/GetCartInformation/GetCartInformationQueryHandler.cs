// using Application.Abstractions.Messaging;
// using Application.DTOs.Order;
// using Application.IServices;
// using Domain.Entities.Customers;
// using Domain.Utilities.Errors;
// using SharedKernel;

// namespace Application.Features.OrderFeature.Queries.GetCartInformation;

// internal sealed class GetCartInformationQueryHandler : IQueryHandler<GetCartInformationQuery, OrderResponse>
// {
//     private readonly IOrderQueryServices _orderQueryServices;
//     public GetCartInformationQueryHandler(IOrderQueryServices orderQueryServices)
//     {
//         _orderQueryServices = orderQueryServices;
//     }

//     public async Task<Result<OrderResponse>> Handle(GetCartInformationQuery request, CancellationToken cancellationToken)
//     {
//         CustomerId customerId = CustomerId.FromGuid(request.CustomerId);
//         var order = await _orderQueryServices.GetCartInformation(customerId);
//         if(order is null)
//         {
//             return Result.Failure<OrderResponse>(OrderError.OrderNotCreated(customerId));
//         }
//         return Result.Success(order);
//     }
// }
