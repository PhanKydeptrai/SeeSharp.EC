using Application.Abstractions.Messaging;
using Application.DTOs.Order;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Queries.GetCartInformationForGuest;

internal sealed class GetCartInformationForGuestQueryHandler : IQueryHandler<GetCartInformationForGuestQuery, OrderResponse>
{
    private readonly IOrderQueryServices _orderqueryServices;
    public GetCartInformationForGuestQueryHandler(IOrderQueryServices orderqueryServices)
    {
        _orderqueryServices = orderqueryServices;
    }

    public async Task<Result<OrderResponse>> Handle(GetCartInformationForGuestQuery request, CancellationToken cancellationToken)
    {
        CustomerId customerId = CustomerId.FromGuid(request.GuestId);
        var order = await _orderqueryServices.GetCartInformation(customerId);
        if(order is null)
        {
            return Result.Failure<OrderResponse>(OrderError.OrderNotCreated(customerId));
        }
        return Result.Success(order);
    }
}
