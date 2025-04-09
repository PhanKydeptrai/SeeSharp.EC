using System.IdentityModel.Tokens.Jwt;
using API.Extentions;
using API.Infrastructure;
using API.Infrastructure.Authorization;
using Application.Features.CustomerFeature.Commands.CustomerChangePassword;
using Application.Features.CustomerFeature.Commands.CustomerConfirmChangePassword;
using Application.Features.CustomerFeature.Commands.CustomerConfirmResetPassword;
using Application.Features.CustomerFeature.Commands.CustomerResetPassword;
using Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithGoogle;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;
using Application.Features.CustomerFeature.Commands.CustomerSignUp;
using Application.Features.CustomerFeature.Commands.CustomerVerifyEmail;
using Application.Features.CustomerFeature.Commands.GenerateGuestToken;
using Application.Features.CustomerFeature.Commands.RevokeAllCustomerRefreshTokens;
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("signin", Name = EndpointName.Customer.SignIn)]
    // [AuthorizeByRole(AuthorizationPolicies.Guest)]
    [ApiKey]
    public async Task<IResult> SignIn([FromBody] CustomerSignInCommand request)
    {
        // string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        // var claims = TokenExtentions.DecodeJwt(token);
        // claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);
        // var result = await _sender.Send(new CustomerSignInCommand(request.Email, request.Password, Guid.Parse(guestId!)));
        var result = await _sender.Send(request);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Get customer profile
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile", Name = EndpointName.Customer.GetProfile)]
    [Authorize]
    [ApiKey]
    public async Task<IResult> GetCustomerProfile()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
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
    [HttpPost("signup", Name = EndpointName.Customer.SignUp)]
    // [AuthorizeByRole(AuthorizationPolicies.Guest)]
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
    [HttpPost("reset-password", Name = EndpointName.Customer.ResetPassword)]
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

    /// <summary>
    /// Cho phép khách hàng thay đổi mật khẩu của mình
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("change-password", Name = EndpointName.Customer.ChangePassword)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    [ApiKey]
    public async Task<IResult> CustomerChangePassword([FromBody] CustomerChangePasswordRequest request)
    {

        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
    
        var result = await _sender.Send(new CustomerChangePasswordCommand(
            Guid.Parse(sub!),
            request.currentPassword,
            request.newPassword,
            request.repeatNewPassword));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Xác nhận thay đổi mật khẩu của khách hàng
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    [HttpGet("confirm-change-password/{verificationTokenId:guid}", Name = EndpointName.Customer.ChangePasswordConfirm)]
    public async Task<IResult> ChangePasswordConfirm([FromRoute] Guid verificationTokenId)
    {
        var result = await _sender.Send(new CustomerConfirmChangePasswordCommand(verificationTokenId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép khách hàng đăng nhập lại bằng refresh token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpGet("refresh-token", Name = EndpointName.Customer.SignInWithRefreshToken)]
    [AuthorizeByRole(AuthorizationPolicies.Guest)]
    [ApiKey]
    public async Task<IResult> SignInWithRefreshToken([FromBody] CustomerSignInWithRefreshTokenCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi refresh token của khách hàng
    /// </summary>
    /// <param name="userId">ID của khách hàng</param>
    /// <returns></returns>
    [HttpDelete("{userId:guid}/refresh-token")]
    [EndpointName(EndpointName.Customer.RevokeRefreshToken)]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeRefreshToken([FromRoute] Guid userId)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        claims.TryGetValue(JwtRegisteredClaimNames.Jti, out var jti);

        if (sub != userId.ToString()) 
        {
            return Results.Unauthorized();
        }

        var result = await _sender.Send(new CustomerRevokeRefreshTokenCommand(jti!));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi refresh token của khách hàng (lấy userId từ token, Admin và khách đều có thể dùng chung)
    /// </summary>
    /// <returns></returns>
    [HttpDelete("refresh-token/current", Name = EndpointName.Customer.RevokeRefreshToken + "FromToken")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeCurrentRefreshToken()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        claims.TryGetValue(JwtRegisteredClaimNames.Jti, out var jti);

        var result = await _sender.Send(new CustomerRevokeRefreshTokenCommand(jti!));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi tất cả refresh tokens của người dùng (Tất cẩ đều dùng được)
    /// </summary>
    /// <param name="userId">ID user</param>
    /// <returns></returns>
    [HttpDelete("{userId:guid}/refresh-tokens")]
    [EndpointName(EndpointName.Customer.RevokeRefreshTokens)]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeAllRefreshTokens([FromRoute] Guid userId)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        
        if (sub != userId.ToString())
        {
            return Results.Unauthorized();
        }

        var result = await _sender.Send(new RevokeAllCustomerRefreshTokensCommand(userId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi tất cả refresh tokens của khách hàng (lấy userId từ token)
    /// </summary>
    /// <returns></returns>
    [HttpDelete("refresh-tokens/current", Name = EndpointName.Customer.RevokeRefreshTokens + "FromToken")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeAllCurrentRefreshTokens()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        
        var result = await _sender.Send(new RevokeAllCustomerRefreshTokensCommand(new Guid(sub!)));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép khách hàng đăng nhập bằng tài khoản Google
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("google-login/{token}")]
    [ApiKey]
    public async Task<IResult> SignInWithGoogle([FromRoute] string token)
    {
        var result = await _sender.Send(new CustomerSignInWithGoogleCommand(token));
        return result.Match(Results.Ok, CustomResults.Problem); 
    }

    /// <summary>
    /// Cho phép client lấy guest token
    /// </summary>
    /// <returns></returns>
    [HttpGet("guest-token")]
    [ApiKey]
    public async Task<IResult> GetGuestToken()
    {
        var result = await _sender.Send(new GenerateGuestTokenCommand());
        return result.Match(Results.Ok, CustomResults.Problem); 
    }


    // Additional customer endpoints could be added here
    // - SignInWithGoogle
    // - SignInWithRefreshToken

    // - EmailVerification
    // - RevokeCustomerRefreshToken
    // - RevokeAllCustomerRefreshTokens
}