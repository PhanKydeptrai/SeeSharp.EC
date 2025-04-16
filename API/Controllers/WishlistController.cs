using API.Extentions;
using API.Infrastructure;
using Application.Features.WishItemFeature.Commands;
using Application.Features.WishItemFeature.Commands.DeleteWishItemFromWishList;
using Application.Features.WishItemFeature.Queries.GetWishList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/wishitems")]
[ApiController]
// [ApiKey]
public sealed class WishlistController : ControllerBase
{
    private readonly ISender _sender;
    
    public WishlistController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lấy danh sách sản phẩm yêu thích của khách hàng
    /// </summary>
    /// <param name="productStatusFilter"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet(Name = EndpointName.Wishlist.GetWishList)]
    [Authorize]
    public async Task<IResult> GetWishList(
        [FromQuery] string? productStatusFilter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var query = new GetWishListQuery(
            productStatusFilter, 
            searchTerm, 
            sortColumn, 
            sortOrder, 
            page, 
            pageSize, 
            Guid.Parse(customerId!));

        var result = await _sender.Send(query);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Thêm một sản phẩm vào danh sách yêu thích
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpPost("{productId:guid}")]
    [EndpointName(EndpointName.Wishlist.AddWishList)]
    [Authorize]
    public async Task<IResult> AddWishList([FromRoute] Guid productId)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

        var command = new AddWishListCommand(productId, Guid.Parse(customerId!));
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Xóa một sản phẩm khỏi danh sách yêu thích
    /// </summary>
    /// <param name="wishItemId"></param>
    /// <returns></returns>
    [HttpDelete("{wishItemId:guid}")]
    [Authorize]
    public async Task<IResult> DeleteWishItem([FromRoute] Guid wishItemId)
    {
        var result = await _sender.Send(new DeleteWishItemFromWishListCommand(wishItemId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
} 