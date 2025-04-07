using System.IdentityModel.Tokens.Jwt;
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerConfirmResetPassword;
using Application.Features.CustomerFeature.Commands.CustomerResetPassword;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using Application.Features.CustomerFeature.Commands.CustomerSignUp;
using Application.Features.CustomerFeature.Commands.CustomerVerifyEmail;
using Application.Features.CustomerFeature.Queries.GetCustomerProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/customers")]
[ApiController]
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
    [ApiKey]
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
    [ApiKey]
    public async Task<IResult> GetCustomerProfile()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        var result = await _sender.Send(new GetCustomerProfileQuery(new Guid(sub!)));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Đăng ký tài khoản khách hàng
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("signup")]
    [EndpointName(EndpointName.Customer.SignUp)]
    [ApiKey]
    public async Task<IResult> SignUp([FromBody] CustomerSignUpCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép khách hàng gửi yêu cầu thay đổi mật khẩu  
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [EndpointName(EndpointName.Customer.ResetPassword)]
    [ApiKey]
    public async Task<IResult> CustomerResetPassword([FromBody] CustomerResetPasswordCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }


    /// <summary>
    /// Xác nhận thay đổi mật khẩu của khách hàng
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    [HttpGet("confirm-reset-password/{verificationTokenId:guid}", Name = EndpointName.Customer.ResetPasswordConfirm)]
    public async Task<IResult> CustomerConfirmChangePassword([FromRoute] Guid verificationTokenId)
    {
        var result = await _sender.Send(new CustomerConfirmResetPasswordCommand(verificationTokenId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép khách hàng xác thực tài khoản của mình
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    [HttpGet("verify-email/{verificationTokenId:guid}", Name = EndpointName.Customer.Verify)]
    // [EndpointName(EndpointName.Customer.Verify)]
    public async Task<IResult> CustomerVerifyEmail([FromRoute] Guid verificationTokenId)
    {
        var result = await _sender.Send(new CustomerVerifyEmailCommand(verificationTokenId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    [HttpGet("confirm-email/{verificationTokenId:guid}", Name = EndpointName.Customer.ChangePasswordConfirm)]
    public async Task<IResult> CustomerConfirmEmail([FromRoute] Guid verificationTokenId)
    {
        var result = await _sender.Send(new CustomerConfirmResetPasswordCommand(verificationTokenId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
    // Additional customer endpoints could be added here
    // - SignInWithGoogle
    // - SignInWithRefreshToken
    // - CustomerChangePassword

    // - EmailVerification
    // - RevokeCustomerRefreshToken
    // - RevokeAllCustomerRefreshTokens
} 