using API.Extentions;
using API.Infrastructure;
using API.Infrastructure.Authorization;
using Application.Features.CustomerFeature.Commands.CustomerChangePassword;
using Application.Features.CustomerFeature.Commands.CustomerConfirmChangePassword;
using Application.Features.CustomerFeature.Commands.CustomerConfirmResetPassword;
using Application.Features.CustomerFeature.Commands.CustomerResetPassword;
using Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithExternal;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;
using Application.Features.CustomerFeature.Commands.CustomerSignUp;
using Application.Features.CustomerFeature.Commands.CustomerVerifyEmail;
using Application.Features.CustomerFeature.Commands.GenerateGuestToken;
using Application.Features.CustomerFeature.Commands.RevokeAllCustomerRefreshTokens;
using Application.Features.CustomerFeature.Commands.UpdateCustomerProfile;
using Application.Features.CustomerFeature.Queries.GetAllCustomer;
using Application.Features.CustomerFeature.Queries.GetCustomerProfile;
using Application.Features.CustomerFeature.Queries.GetCustomerProfileById;
using AspNet.Security.OAuth.Discord;
using AspNet.Security.OAuth.GitHub;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
    /// Cho phép khách hàng đăng nhập vào hệ thống
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("signin", Name = EndpointName.Customer.SignIn)]
    // [AuthorizeByRole(AuthorizationPolicies.Guest)]
    // [ApiKey]
    public async Task<IResult> SignIn(
        [FromBody] CustomerSignInRequest request)
    {
        var result = await _sender.Send(new CustomerSignInCommand(
            request.Email,
            request.Password));

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Get customer profile
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile", Name = EndpointName.Customer.GetProfile)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    //[ApiKey]
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
    // //[ApiKey]
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
    // //[ApiKey]
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
        if (result.IsFailure)
        {
            return Results.Redirect("https://localhost:7115/password-reset-failed");
        }
        return Results.Redirect("https://localhost:7115/password-reset-success");
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
        if (result.IsFailure)
        {
            return Results.Redirect("https://localhost:7115/authentication-failed");
        }
        return Results.Redirect("https://localhost:7115/authentication-success");
    }

    /// <summary>
    /// Cho phép khách hàng thay đổi mật khẩu của mình
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("change-password", Name = EndpointName.Customer.ChangePassword)]
    [AuthorizeByRole(AuthorizationPolicies.SubscribedOnly)]
    // //[ApiKey]
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
        if (result.IsFailure)
        {
            return Results.Redirect("https://localhost:7115/change-password-success");
        }
        return Results.Redirect("https://localhost:7115/change-password-failed");
    }

    /// <summary>
    /// Cho phép khách hàng đăng nhập lại bằng refresh token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpGet("refresh-token", Name = EndpointName.Customer.SignInWithRefreshToken)]
    //[ApiKey]
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
    //[ApiKey]
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
    //[ApiKey]
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
    //[ApiKey]
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
    //[ApiKey]
    public async Task<IResult> RevokeAllCurrentRefreshTokens()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

        var result = await _sender.Send(new RevokeAllCustomerRefreshTokensCommand(new Guid(sub!)));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép client lấy guest token
    /// </summary>
    /// <returns></returns>
    [HttpGet("guest-token")]
    //[ApiKey]
    public async Task<IResult> GetGuestToken()
    {
        var result = await _sender.Send(new GenerateGuestTokenCommand());
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    #region Google Authen
    /// <summary>
    /// Bắt đầu quá trình đăng nhập bằng Google.
    /// </summary>
    /// <returns>Chuyển hướng đến trang đăng nhập của Google.</returns>
    [HttpGet("signin-google")]
    public async Task<IActionResult> ExternalLoginGoogle()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback") };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Xử lý callback sau khi đăng nhập bằng Google.
    /// </summary>
    /// <returns>Chuyển hướng về frontend với token JWT (nếu thành công đăng nhập thành công).</returns>
    [HttpGet("external-login-callback")]
    public async Task<IResult> ExternalLoginCallback()
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticationResult.Succeeded)
        {
            return Results.Redirect("https://localhost:7115/login");
        }
        var claims = authenticationResult.Principal.Claims;
        var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var idtoken = claims.FirstOrDefault(x => x.Type == "id_token")?.Value;
        var result = await _sender.Send(new CustomerSignInWithExternalCommand(name!, email!));

        var redirectUrl = $"https://localhost:7115/login?token={result.Value.accessToken}&?rtoken={result.Value.refreshToken}";

        return Results.Redirect(redirectUrl);
    }
    #endregion

    #region Github Authen
    /// <summary>
    /// Bắt đầu quá trình đăng nhập bằng Github.
    /// </summary>
    /// <returns>Chuyển hướng đến trang đăng nhập của Github.</returns>
    [HttpGet("signin-github")]
    public async Task<IActionResult> ExternalLoginGithub()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallbackForGit") };
        return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Xử lý callback sau khi đăng nhập bằng Github.
    /// </summary>
    /// <returns></returns>
    [HttpGet("signin-github-callback-url")]
    public async Task<IResult> ExternalLoginCallbackForGit()
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(GitHubAuthenticationDefaults.AuthenticationScheme);

        if (!authenticationResult.Succeeded)
        {
            return null;
        }

        // Hoặc lấy từ AuthenticationProperties

        var claims = authenticationResult.Principal.Claims;
        var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var result = await _sender.Send(new CustomerSignInWithExternalCommand(name!, email!));

        var redirectUrl = $"https://localhost:7115/login?token={result.Value.accessToken}&?rtoken={result.Value.refreshToken}";

        return Results.Redirect(redirectUrl);
    }
    #endregion

    #region Discord Authen

    /// <summary>
    /// Đăng nhập discord
    /// </summary>
    /// <returns>Chuyển hướng đến trang đăng nhập của Discord</returns>
    [HttpGet("signin-discord")]
    [Obsolete]
    public async Task<IActionResult> ExternalLoginDiscord()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallbackForDiscord") };
        return Challenge(properties, DiscordAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("signin-discord-callback")]
    [Obsolete]
    public async Task<IResult> ExternalLoginCallbackForDiscord([FromQuery] string? code)
    {

        var authenticationResult = await HttpContext
            .AuthenticateAsync(DiscordAuthenticationDefaults.AuthenticationScheme);

        if (!authenticationResult.Succeeded)
        {
            return null;
        }

        // Hoặc lấy từ AuthenticationProperties

        var claims = authenticationResult.Principal.Claims;
        // var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        // var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        // var result = await _sender.Send(new CustomerSignInWithGoogleCommand(name!, email!));

        // var redirectUrl = $"https://localhost:7192/?data=";


        // return Results.Redirect(redirectUrl);
        return Results.Ok(new { claims = claims.ToList() });
    }


    #endregion

    #region Facebook Authen
    /// <summary>
    /// Bắt đầu quá trình đăng nhập bằng Facebook.
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    [HttpGet("signin-facebook")]
    public async Task<IActionResult> ExternalLoginFacebook()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallbackForFacebook") };
        return Challenge(properties, FacebookDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Xử lý callback sau khi đăng nhập bằng Facebook.
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    [HttpGet("external-facebook-login-callback")]
    public async Task<IResult> ExternalLoginCallbackForFacebook()
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

        if (!authenticationResult.Succeeded)
        {
            return null;
        }

        // Hoặc lấy từ AuthenticationProperties

        var claims = authenticationResult.Principal.Claims;
        var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var result = await _sender.Send(new CustomerSignInWithExternalCommand(name!, email!));

        var redirectUrl = $"https://localhost:7192/?data=";

        return Results.Redirect(redirectUrl);
    }
    #endregion

    /// <summary>
    /// Cập nhật thông tin cá nhân của khách hàng
    /// </summary>
    /// <returns></returns>
    [HttpPut("profile", Name = EndpointName.Customer.UpdateProfile)]
    [Authorize]
    public async Task<IResult> UpdateCustomerProfile([FromForm] UpdateCustomerProfileRequest request)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext)!;
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

        var userId = Guid.Parse(sub!);

        // Map request to command and add the user ID from token
        var command = new UpdateCustomerProfileCommand(
            userId,
            request.UserName,
            request.PhoneNumber,
            request.Gender,
            request.DateOfBirth != null ? DateOnly.Parse(request.DateOfBirth) : null,
            request.ImageFile
        );

        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);

    }

    [HttpGet]
    [Authorize]
    public async Task<IResult> GetAllCustomerProfile(
        [FromQuery] string? statusFilter,
        [FromQuery] string? customerTypeFilter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _sender.Send(
            new GetAllCustomerQuery(
                statusFilter, 
                customerTypeFilter, 
                searchTerm, 
                sortColumn, 
                sortOrder, 
                page, 
                pageSize));

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy thông tin chi tiết của khách hàng theo ID
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId:guid}")]
    [AuthorizeByRole(AuthorizationPolicies.AllEmployee)]
    public async Task<IResult> GetCustomerProfileById([FromRoute] Guid userId)
    {
        var result = await _sender.Send(new GetCustomerProfileByIdQuery(userId));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

}