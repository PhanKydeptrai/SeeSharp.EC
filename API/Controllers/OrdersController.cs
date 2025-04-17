using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.AddProductToOrder;
using Application.Features.OrderFeature.Commands.DeleteOrderDetail;
using Application.Features.OrderFeature.Commands.UpdateOrderDetail;
using Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using MediatR;
using Application.Features.OrderFeature.Queries.GetOrderByOrderId;
using Application.Features.OrderFeature.Queries.GetCartInformation;
using Application.Features.OrderFeature.Queries.GetAllOrderForAdmin;
using API.Infrastructure.Authorization;
using Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;
using Application.Features.OrderFeature.Commands.AddProductToOrderForGuest;
using Application.Features.OrderFeature.Queries.GetMakePaymentResponse;
using Application.Features.OrderFeature.Commands.MakePaymentForGuest;
using Application.Features.OrderFeature.Commands.DeleteOrderTransaction;
using Microsoft.IdentityModel.Tokens;
using Application.Features.OrderFeature.Commands.PayOrderWithVnPay;
using Application.DTOs.Payment;
using Application.Features.OrderFeature.Commands.VnPayReturnUrl;
using Application.Features.OrderFeature.Queries.GetOrderHistoryForCustomer;
using Application.Features.OrderFeature.Commands.PayOrderWithCOD;

namespace API.Controllers;

[Route("api/orders")]
[ApiController]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lấy tất cả đơn hàng của khách hàng (Dùng cho khách hàng)
    /// </summary>
    /// <returns></returns>
    [HttpGet("customer", Name = EndpointName.Order.GetAllOrderForCustomer)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> GetAllOrdersForCustomer(
        [FromQuery] string? statusFilter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
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
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> AddProductToOrder([FromBody] AddProductToOrderRequest request)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
        if (customerId is null) return Results.Unauthorized();
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
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
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
    [HttpPut("details/{orderDetailId:guid}", Name = EndpointName.Order.UpdateOrderDetail)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOrGuest)]
    public async Task<IResult> UpdateOrderDetail(
        [FromRoute] Guid orderDetailId,
        [FromBody] UpdateOrderDetailRequest request)
    {
        var result = await _sender.Send(new UpdateOrderDetailCommand(orderDetailId, request.Quantity));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy thông tin đơn hàng theo orderId (Dùng cho khách hàng)
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    // Usecase 1: Admin want to see the details of an order. | Requires Admin Role and Order Status is not "New"

    [HttpGet("{orderId:guid}", Name = EndpointName.Order.GetOrderByOrderId)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> GetOrderByOrderId(
        [FromRoute] Guid orderId)
    {
        var query = new GetOrderByOrderIdQuery(orderId);
        var result = await _sender.Send(query);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy thông tin giỏ hàng của khách
    /// </summary>
    /// <returns></returns>
    [HttpGet("cart", Name = EndpointName.Order.GetCartInformation)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOrGuest)]
    public async Task<IResult> GetCartInformation()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        string? id = string.Empty;
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out id);

        if (id.IsNullOrEmpty())
        {
            claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out id);
        }
        var result = await _sender.Send(new GetCartInformationQuery(new Guid(id!)));
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
    [HttpGet("admin", Name = EndpointName.Order.GetAllOrderForAdmin)]
    [AuthorizeByRole(AuthorizationPolicies.Admin)]
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

    /// <summary>
    /// Tạo ordertransaction cho hoá đơn (Cho khách hàng đã đăng ký)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    //[ApiKey]
    [HttpPost("make-payment")]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> MakePaymentForSubscriber([FromBody] MakePaymentForSubscriberRequest request)
    {
        string? token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token!);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var result = await _sender.Send(new MakePaymentForSubscriberCommand(
            new Guid(customerId!),
            request.voucherCode,
            request.ShippingInformationId,
            request.FullName,
            request.PhoneNumber,
            request.Province,
            request.District,
            request.Ward,
            request.SpecificAddress));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    // [HttpPut("make-payment")]
    // [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    // public async Task<IResult> UpdateMakePaymentForSubscriber([FromBody] MakePaymentForSubscriberRequest request)
    // {
    //     string? token = TokenExtentions.GetTokenFromHeader(HttpContext);
    //     var claims = TokenExtentions.DecodeJwt(token!);
    //     claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
        
    //     var result = await _sender.Send(new MakePaymentForSubscriberCommand(
    //         new Guid(customerId!),
    //         request.voucherCode,
    //         request.ShippingInformationId,
    //         request.FullName,
    //         request.PhoneNumber,
    //         request.Province,
    //         request.District,
    //         request.Ward,
    //         request.SpecificAddress));

    //     return result.Match(Results.NoContent, CustomResults.Problem);
    // }

    /// <summary>
    ///  (Cho khách hàng chưa đăng ký)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("guest")]
    [AuthorizeByRole(AuthorizationPolicies.Guest)]
    //[ApiKey]
    public async Task<IResult> AddProductToOrderForGuest(
        [FromBody] AddProductToOrderForGuestRequest request)
    {
        string? token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token!);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);

        var command = new AddProductToOrderForGuestCommand(request.ProductVariantId, Guid.Parse(guestId!), request.Quantity);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy thông tin thanh toán cho khách hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet("make-payment", Name = EndpointName.Order.GetMakePaymentResponse)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    //[ApiKey]
    public async Task<IResult> GetMakePaymentResponse()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var result = await _sender.Send(new GetMakePaymentResponseQuery(new Guid(customerId!)));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy thông tin thanh toán cho khách hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet("make-payment/guest")]
    [AuthorizeByRole(AuthorizationPolicies.Guest)]
    //[ApiKey]
    public async Task<IResult> GetMakePaymentResponseForGuest()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);

        var result = await _sender.Send(new GetMakePaymentResponseQuery(new Guid(guestId!)));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Tạo ordertransaction cho hoá đơn (Cho khách hàng chưa đăng ký)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("make-payment/guest")]
    [AuthorizeByRole(AuthorizationPolicies.Guest)]
    //[ApiKey]
    public async Task<IResult> MakePaymentForGuest([FromBody] MakePaymentForGuestRequest request)
    {
        string? token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token!);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);

        var command = new MakePaymentForGuestCommand(
            new Guid(guestId!),
            request.Email,
            request.FullName,
            request.PhoneNumber,
            request.Province,
            request.District,
            request.Ward,
            request.SpecificAddress);

        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Delete an order transaction for a customer
    /// </summary>
    /// <returns></returns>
    //[ApiKey]
    [HttpDelete("transaction")]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOrGuest)]
    public async Task<IResult> DeleteOrderTransaction()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        string? id = string.Empty;

        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out id);
        if (id.IsNullOrEmpty())
        {
            claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out id);
        }

        var result = await _sender.Send(new DeleteOrderTransactionCommand(new Guid(id!)));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    #region Thanh toán
    /// <summary>
    /// Thanh toán đơn hàng với VnPay
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("vnpay/{orderId:guid}")]
    // [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> PayOrderWithVnPay([FromRoute] Guid orderId)
    {
        var command = new PayOrderWithVnPayCommand(orderId);
        var result = await _sender.Send(command);
        return Results.Redirect(result.Value);
    }
    
    /// <summary>
    /// Thanh toán khi nhận hàng (COD)
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpPost("payorder-cod/{orderId:guid}")]
    public async Task<IResult> PayorderWithCOD([FromRoute] Guid orderId)
    { 
        var result = await _sender.Send(new PayOrderWithCODCommand(orderId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// VnPay return url
    /// </summary>
    /// <param name="vnp_Amount"></param>
    /// <param name="vnp_BankCode"></param>
    /// <param name="vnp_OrderInfo"></param>
    /// <param name="vnp_ResponseCode"></param>
    /// <param name="vnp_TmnCode"></param>
    /// <param name="vnp_TransactionNo"></param>
    /// <param name="vnp_TxnRef"></param>
    /// <param name="vnp_SecureHash"></param>
    /// <returns></returns>
    [HttpGet("return-url")]
    public async Task<IResult> VnPayReturnUrl(
        [FromQuery(Name = "vnp_Amount")] string vnp_Amount,
        [FromQuery(Name = "vnp_BankCode")] string vnp_BankCode,
        [FromQuery(Name = "vnp_OrderInfo")] string vnp_OrderInfo,
        [FromQuery(Name = "vnp_ResponseCode")] string vnp_ResponseCode,
        [FromQuery(Name = "vnp_TmnCode")] string vnp_TmnCode,
        [FromQuery(Name = "vnp_TransactionNo")] string vnp_TransactionNo,
        [FromQuery(Name = "vnp_TxnRef")] string vnp_TxnRef,
        [FromQuery(Name = "vnp_SecureHash")] string vnp_SecureHash)
    {
        var model = new VnPayReturnModel
        {
            vnp_Amount = vnp_Amount,
            vnp_BankCode = vnp_BankCode,
            vnp_OrderInfo = vnp_OrderInfo,
            vnp_ResponseCode = vnp_ResponseCode,
            vnp_TmnCode = vnp_TmnCode,
            vnp_TransactionNo = vnp_TransactionNo,
            vnp_TxnRef = vnp_TxnRef,
            vnp_SecureHash = vnp_SecureHash
        };
        var result = await _sender.Send(new VnPayReturnUrlCommand(Guid.Parse(model.vnp_TxnRef)));
        if (result.IsSuccess)
        {
            return Results.Redirect("https://localhost:7115/payment-success");
        }
        return Results.Redirect("https://localhost:7115/payment-failed");
    }
    #endregion


    /// <summary>
    /// Lấy lịch sử đơn hàng của khách hàng (Hiển thị tại trang thông tin tài khoản, 
    /// đây là những order đã đặt thành công và có trạng thái khác "Waiting")
    /// </summary>
    /// <returns></returns>
    [HttpGet("history")]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> GetOrderHistoryForCustomer()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        string? customerId = string.Empty;
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out customerId);

        var result = await _sender.Send(new GetOrderHistoryForCustomerQuery(new Guid(customerId!)));

        return result.Match(Results.Ok, CustomResults.Problem);
    }
}