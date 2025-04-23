using Application.Abstractions.Messaging;
using Application.DTOs.Bills;
using Application.IServices;
using Domain.Entities.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Queries.GetOrderHistoryByOrderId;

internal sealed class GetOrderHistoryByOrderIdQueryHandler : IQueryHandler<GetOrderHistoryByOrderIdQuery, BillResponse>
{
    private readonly IOrderQueryServices _orderQueryServices;
    public GetOrderHistoryByOrderIdQueryHandler(IOrderQueryServices orderQueryServices)
    {
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result<BillResponse>> Handle(
        GetOrderHistoryByOrderIdQuery request, 
        CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.OrderId);
        
        var result = await _orderQueryServices.GetBillByOrderId(orderId);

        if(result is null)
        {
            return Result.Failure<BillResponse>(new Error("Order.NotFound", "Order not found", ErrorType.NotFound));
        }

        return Result.Success(result);
    }
}
