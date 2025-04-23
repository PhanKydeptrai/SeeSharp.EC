using Application.Abstractions.Messaging;
using Application.DTOs.Order;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.IRepositories.Orders;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Queries.GetMakePaymentResponse;

internal sealed class GetMakePaymentResponseQueryHandler : IQueryHandler<GetMakePaymentResponseQuery, MakePaymentResponse>
{
    private readonly IOrderQueryServices _orderQueryServices;
    public GetMakePaymentResponseQueryHandler(IOrderQueryServices orderQueryServices)
    {
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result<MakePaymentResponse>> Handle(
        GetMakePaymentResponseQuery request, 
        CancellationToken cancellationToken)
    {
        CustomerId customerId = CustomerId.FromGuid(request.CustomerId);
        var order = await _orderQueryServices.GetCartInformation(customerId);
        
        if (order is null)
        {
            return Result.Failure<MakePaymentResponse>(OrderError.OrderNotCreated(customerId));
        }

        var makePaymentResponse = await _orderQueryServices.GetMakePaymentResponse(customerId);

        if (makePaymentResponse is null)
        {
            return Result.Failure<MakePaymentResponse>(OrderError.OrderNotCreated(customerId));
        }

        return Result.Success(makePaymentResponse);
    }
} 