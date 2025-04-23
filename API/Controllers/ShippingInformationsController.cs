using API.Extentions;
using API.Infrastructure.Authorization;
using Application.Features.ShippingInformationFeature.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using MediatR;
using API.Infrastructure;

namespace API.Controllers;

[Route("api/shipping-informations")]
[ApiController]
public class ShippingInformationsController : ControllerBase
{
    private readonly ISender _sender;

    public ShippingInformationsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lấy thông tin giao hàng mặc định của người dùng
    /// </summary>
    /// <returns></returns>
    [HttpGet("default")]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    public async Task<IResult> GetDefaultShippingInformation()
    {
        string? token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token!);
        claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
        
        var result = await _sender.Send(new GetDefaultShippingInformationQuery(Guid.Parse(customerId!)));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    // [HttpPost]
    // [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    // public async Task<IResult> CreateShippingInformation()
    // {
    //     throw new NotImplementedException();
    // }
}
