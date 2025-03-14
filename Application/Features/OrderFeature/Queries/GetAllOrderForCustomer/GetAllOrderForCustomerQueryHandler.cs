using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.DTOs.Order;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Customers;
using SharedKernel;

namespace Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;

internal sealed class GetAllOrderForCustomerQueryHandler : IQueryHandler<GetAllOrderForCustomerQuery, PagedList<OrderResponse>>
{
    private readonly IOrderQueryServices _orderQueryServices;
    public GetAllOrderForCustomerQueryHandler(IOrderQueryServices orderQueryServices)
    {
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result<PagedList<OrderResponse>>> Handle(
        GetAllOrderForCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _orderQueryServices.GetAllOrderForCustomer(
            CustomerId.FromGuid(request.customerId),
            request.statusFilter,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page,
            request.pageSize);

        return Result.Success(result);
    }
}
