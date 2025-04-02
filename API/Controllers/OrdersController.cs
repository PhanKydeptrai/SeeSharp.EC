using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.AddProductToOrder;
using Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/orders")]
[ApiController]
[ApiKey]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;
    
    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lấy tất cả đơn hàng của khách hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet("customer")]
    [EndpointName(EndpointName.Order.GetAllOrderForCustomer)]
    [Authorize]
    public async Task<IResult> GetAllOrdersForCustomer(
        [FromQuery] string? statusFilter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var query = new GetAllOrderForCustomerQuery(
            new Guid(customerId!),
            statusFilter,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);

        var result = await _sender.Send(query);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Thêm sản phẩm vào giỏ hàng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [EndpointName(EndpointName.Order.AddProductToOrder)]
    [Authorize]
    public async Task<IResult> AddProductToOrder([FromBody] AddProductToOrderRequest request)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var command = new AddProductToOrderCommand(request.ProductVariantId, new Guid(customerId!), request.Quantity);
        var result = await _sender.Send(command);

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    // Additional order endpoints could be added here:
    // - GetOrderByOrderId
    // - UpdateOrderDetail
    // - DeleteOrderdetail
    // - GetAllOrderForAdmin
    // - GetCartInformation
} 