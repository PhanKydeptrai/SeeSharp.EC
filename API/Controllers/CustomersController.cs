using System.IdentityModel.Tokens.Jwt;
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using Application.Features.CustomerFeature.Queries.GetCustomerProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/customers")]
[ApiController]
[ApiKey]
public sealed class CustomersController : ControllerBase
{
    private readonly ISender _sender;
    public CustomersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Sign in a customer
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("signin")]
    [EndpointName(EndpointName.Customer.SignIn)]
    public async Task<IResult> SignIn([FromBody] CustomerSignInCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Get customer profile
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile")]
    [EndpointName(EndpointName.Customer.GetProfile)]
    [Authorize]
    public async Task<IResult> GetCustomerProfile()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        var result = await _sender.Send(new GetCustomerProfileQuery(new Guid(sub!)));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    // Additional customer endpoints could be added here
    // - SignUp
    // - SignInWithGoogle
    // - SignInWithRefreshToken
    // - CustomerChangePassword
    // - CustomerConfirmChangePassword
    // - CustomerResetPassword
    // - CustomerConfirmResetPassword
    // - EmailVerification
    // - RevokeCustomerRefreshToken
    // - RevokeAllCustomerRefreshTokens
} 