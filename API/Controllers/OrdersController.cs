using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.AddProductToOrder;
using Application.Features.OrderFeature.Commands.DeleteOrderDetail;
using Application.Features.OrderFeature.Commands.UpdateOrderDetail;
using Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using MediatR;
using Application.Features.OrderFeature.Queries.GetOrderByOrderId;
using Application.Features.OrderFeature.Queries.GetCartInformation;
using Application.Features.OrderFeature.Queries.GetAllOrderForAdmin;

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

    /// <summary>
    /// Xóa sản phẩm khỏi giỏ hàng
    /// </summary>
    /// <param name="orderDetailId"></param>
    /// <returns></returns>
    [HttpDelete("details/{orderDetailId:guid}")]
    [EndpointName(EndpointName.Order.DeleteOrderDetail)]
    public async Task<IResult> DeleteOrderdetail([FromRoute] Guid orderDetailId)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var result = await _sender.Send(new DeleteOrderDetailCommand(orderDetailId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cập nhật số lượng sản phẩm trong giỏ hàng
    /// </summary>
    /// <param name="orderDetailId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("details/{orderDetailId:guid}")]
    [EndpointName(EndpointName.Order.UpdateOrderDetail)]
    public async Task<IResult> UpdateOrderDetail(
        [FromRoute] Guid orderDetailId,
        [FromBody] UpdateOrderDetailRequest request)
    {
        var result = await _sender.Send(new UpdateOrderDetailCommand(orderDetailId, request.Quantity));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Use this endpoint to get order details by order id. 
    /// Usecase 1: Admin want to see the details of an order. | Requires Admin Role and Order Status is not "New"
    /// </summary>

    [HttpGet("{orderId:guid}")]
    [EndpointName(EndpointName.Order.GetOrderByOrderId)]
    public async Task<IResult> GetOrderByOrderId(
        [FromRoute] Guid orderId)
    {
        var query = new GetOrderByOrderIdQuery(orderId);
        var result = await _sender.Send(query);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Get cart information for customer
    /// </summary>
    /// <returns></returns>
    [HttpGet("cart")]
    [EndpointName(EndpointName.Order.GetCartInformation)]
    [Authorize]
    public async Task<IResult> GetCartInformation()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var result = await _sender.Send(new GetCartInformationQuery(new Guid(customerId!)));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Get all orders for admin
    /// </summary>
    /// <param name="statusFilter"></param>
    /// <param name="customerFilter"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("admin")]
    [EndpointName(EndpointName.Order.GetAllOrderForAdmin)]
    public async Task<IResult> GetAllOrderForAdmin(
        [FromQuery] string? statusFilter,
        [FromQuery] string? customerFilter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _sender.Send(new GetAllOrderForAdminQuery(
            statusFilter,
            customerFilter,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize));

        return result.Match(Results.Ok, CustomResults.Problem);
    }

}