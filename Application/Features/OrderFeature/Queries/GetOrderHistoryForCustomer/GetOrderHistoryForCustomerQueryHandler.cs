using Application.Abstractions.Messaging;
using Application.DTOs.Order;
using Application.IServices;
using Domain.Entities.Customers;
using SharedKernel;

namespace Application.Features.OrderFeature.Queries.GetOrderHistoryForCustomer;

internal sealed class GetOrderHistoryForCustomerQueryHandler : IQueryHandler<GetOrderHistoryForCustomerQuery, List<OrderHistoryResponse>>
{
    private readonly IOrderQueryServices _orderQueryServices;
    public GetOrderHistoryForCustomerQueryHandler(IOrderQueryServices orderQueryServices)
    {
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result<List<OrderHistoryResponse>>> Handle(
        GetOrderHistoryForCustomerQuery request, 
        CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.CustomerId);

        var result = await _orderQueryServices.GetOrderHistoryForCustomer(customerId);
        return Result.Success(result);
    }
}