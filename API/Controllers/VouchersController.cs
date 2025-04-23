using API.Extentions;
using API.Infrastructure;
using API.Infrastructure.Authorization;
using Application.Features.VoucherFeature.Commands.CreateNewVoucher;
using Application.Features.VoucherFeature.Queries.GetAllCustomerVoucher;
using Application.Features.VoucherFeature.Queries.GetAllVoucher;
using Application.Features.VoucherFeature.Queries.GetVoucherById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/vouchers")]
[ApiController]
public class VouchersController : ControllerBase
{
    private readonly ISender _sender;

    public VouchersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Creates a new voucher
    /// </summary>
    /// <param name="command">The voucher data</param>
    /// <returns>Success or error result</returns>
    [HttpPost(Name = EndpointName.Voucher.Create)]
    [AuthorizeByRole(AuthorizationPolicies.AdminOnly)]
    //[ApiKey]
    public async Task<IResult> CreateVoucher([FromBody] CreateNewVoucherCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
    
    /// <summary>
    /// Gets a voucher by its ID
    /// </summary>
    /// <param name="voucherId">The voucher identifier</param>
    /// <returns>The voucher details</returns>
    [HttpGet("{voucherId:guid}", Name = EndpointName.Voucher.GetById)]
    [AuthorizeByRole(AuthorizationPolicies.AdminOnly)]
    //[ApiKey]
    public async Task<IResult> GetVoucherById(Guid voucherId)
    {
        var result = await _sender.Send(new GetVoucherByIdQuery(voucherId));
        return result.Match(Results.Ok, CustomResults.Problem);
    }
    
    /// <summary>
    /// Lấy tất cả voucher (Cho Admin)
    /// </summary>
    /// <param name="voucherTypeFilter">Filter by voucher type (Direct or Percentage)</param>
    /// <param name="statusFilter">Filter by status</param>
    /// <param name="searchTerm">Search term for voucher name or code</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="sortOrder">Sort order (asc or desc)</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paged list of vouchers</returns>
    [HttpGet(Name = EndpointName.Voucher.GetAll)]
    [AuthorizeByRole(AuthorizationPolicies.AdminOnly)]
    //[ApiKey]
    public async Task<IResult> GetAllVouchers(
        [FromQuery] string? voucherTypeFilter,
        [FromQuery] string? statusFilter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _sender.Send(new GetAllVoucherQuery(
            voucherTypeFilter,
            statusFilter,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize));
            
        return result.Match(Results.Ok, CustomResults.Problem);
    }
    
    /// <summary>
    /// Lấy tất cả voucher của khách hàng với các tham số lọc, sắp xếp và phân trang
    /// </summary>
    /// <param name="voucherTypeFilter">Filter by voucher type (Direct or Percentage)</param>
    /// <param name="statusFilter">Filter by status</param>
    /// <param name="searchTerm">Search term for voucher name or code</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="sortOrder">Sort order (asc or desc)</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paged list of customer vouchers</returns>
    [HttpGet("customer", Name = EndpointName.Voucher.GetAllForCustomer)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    //[ApiKey]
    public async Task<IResult> GetAllCustomerVouchers(
        [FromQuery] string? voucherTypeFilter,
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

        var result = await _sender.Send(new GetAllCustomerVoucherQuery(
            new Guid(customerId!),
            voucherTypeFilter,
            statusFilter,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize));
            
        return result.Match(Results.Ok, CustomResults.Problem);
    }
}