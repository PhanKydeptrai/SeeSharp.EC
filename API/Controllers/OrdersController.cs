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
    /// Get all orders for the current customer
    /// </summary>
    /// <returns></returns>
    [HttpGet("customer")]
    [ActionName(EndpointName.Order.GetAllOrderForCustomer)]
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
    /// Add a product to order
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName(EndpointName.Order.AddProductToOrder)]
    [Authorize]
    public async Task<IResult> AddProductToOrder([FromBody] AddProductToOrderRequest request)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var command = new AddProductToOrderCommand(request.ProductId, new Guid(customerId!), request.Quantity);
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