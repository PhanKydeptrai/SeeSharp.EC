using Application.Abstractions.Messaging;
using Application.DTOs.Order;
using Application.Features.Pages;
using Application.IServices;
using SharedKernel;

namespace Application.Features.OrderFeature.Queries.GetAllOrderForAdmin;

internal sealed class GetAllOrderForAdminQueryHandler : IQueryHandler<GetAllOrderForAdminQuery, PagedList<OrderResponse>>
{
    private readonly IOrderQueryServices _orderQueryServices;
    public GetAllOrderForAdminQueryHandler(
        IOrderQueryServices orderQueryServices)
    {
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result<PagedList<OrderResponse>>> Handle(
        GetAllOrderForAdminQuery request, 
        CancellationToken cancellationToken)
    {
        var orders = await _orderQueryServices.GetAllOrderForAdmin(
            request.orderStatusFilter,
            request.customerFilter, 
            request.searchTerm, 
            request.sortColumn, 
            request.sortOrder, 
            request.page, 
            request.pageSize);

        return Result.Success(orders);
    }
}
