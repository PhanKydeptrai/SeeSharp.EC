using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.CreateNewEmployee;
using Application.Features.EmployeeFeature.Commands.EmployeeChangePassword;
using Application.Features.EmployeeFeature.Commands.EmployeeConfirmChangePassword;
using Application.Features.EmployeeFeature.Commands.EmployeeConfirmResetPassword;
using Application.Features.EmployeeFeature.Commands.EmployeeResetPassword;
using Application.Features.EmployeeFeature.Commands.EmployeeRevokeRefreshToken;
using Application.Features.EmployeeFeature.Commands.EmployeeSignIn;
using Application.Features.EmployeeFeature.Commands.EmployeeSignInWithRefreshToken;
using Application.Features.EmployeeFeature.Commands.RevokeAllEmployeeRefreshTokens;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Tạo nhân viên mới
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="dateOfBirth"></param>
    /// <param name="imageFile"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiKey]
    [Authorize]
    public async Task<IResult> CreateEmployee(
        [FromForm] string userName,
        [FromForm] string email,
        [FromForm] string phoneNumber,
        [FromForm] DateTime? dateOfBirth,
        IFormFile? imageFile)
    {
        var result = await _sender.Send(new CreateNewEmployeeCommand(userName, email, phoneNumber, dateOfBirth, imageFile));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Đăng nhập nhân viên
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("sign-in")]
    [ApiKey]
    public async Task<IResult> SignIn([FromBody] EmployeeSignInCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.Ok, CustomResults.Problem);
    }   

    /// <summary>
    /// Cho phép nhân viên đăng nhập lại bằng refresh token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("refresh-token", Name = "EmployeeSignInWithRefreshToken")]
    [ApiKey]
    public async Task<IResult> SignInWithRefreshToken([FromBody] EmployeeSignInWithRefreshTokenCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép nhân viên gửi yêu cầu đặt lại mật khẩu
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [EndpointName("EmployeeResetPassword")]
    [ApiKey]
    public async Task<IResult> EmployeeResetPassword([FromBody] EmployeeResetPasswordCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Xác nhận đặt lại mật khẩu của nhân viên
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    [HttpGet("confirm-reset-password/{verificationTokenId:guid}", Name = "EmployeeResetPasswordConfirm")]
    public async Task<IResult> EmployeeConfirmResetPassword([FromRoute] Guid verificationTokenId)
    {
        var result = await _sender.Send(new EmployeeConfirmResetPasswordCommand(verificationTokenId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cho phép nhân viên thay đổi mật khẩu của mình
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("change-password", Name = "EmployeeChangePassword")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> EmployeeChangePassword([FromBody] EmployeeChangePasswordRequest request)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
    
        var result = await _sender.Send(new EmployeeChangePasswordCommand(
            Guid.Parse(sub!),
            request.currentPassword,
            request.newPassword,
            request.repeatNewPassword));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Xác nhận thay đổi mật khẩu của nhân viên
    /// </summary>
    /// <param name="verificationTokenId"></param>
    /// <returns></returns>
    [HttpGet("confirm-change-password/{verificationTokenId:guid}", Name = "EmployeeChangePasswordConfirm")]
    public async Task<IResult> EmployeeConfirmChangePassword([FromRoute] Guid verificationTokenId)
    {
        var result = await _sender.Send(new EmployeeConfirmChangePasswordCommand(verificationTokenId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
    
    /// <summary>
    /// Thu hồi refresh token của nhân viên
    /// </summary>
    /// <param name="userId">ID của nhân viên</param>
    /// <returns></returns>
    [HttpDelete("{userId:guid}/refresh-token")]
    [EndpointName("EmployeeRevokeRefreshToken")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeRefreshToken([FromRoute] Guid userId)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        claims.TryGetValue(JwtRegisteredClaimNames.Jti, out var jti);

        if (sub != userId.ToString()) 
        {
            return Results.Unauthorized();
        }

        var result = await _sender.Send(new EmployeeRevokeRefreshTokenCommand(jti!));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi refresh token của nhân viên (lấy userId từ token)
    /// </summary>
    /// <returns></returns>
    [HttpDelete("refresh-token/current")]
    [EndpointName("EmployeeRevokeRefreshTokenFromToken")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeCurrentRefreshToken()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        claims.TryGetValue(JwtRegisteredClaimNames.Jti, out var jti);

        var result = await _sender.Send(new EmployeeRevokeRefreshTokenCommand(jti!));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi tất cả refresh tokens của nhân viên
    /// </summary>
    /// <param name="userId">ID của nhân viên</param>
    /// <returns></returns>
    [HttpDelete("{userId:guid}/refresh-tokens")]
    [EndpointName("EmployeeRevokeRefreshTokens")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeAllRefreshTokens([FromRoute] Guid userId)
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        
        if (sub != userId.ToString())
        {
            return Results.Unauthorized();
        }

        var result = await _sender.Send(new RevokeAllEmployeeRefreshTokensCommand(userId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Thu hồi tất cả refresh tokens của nhân viên (lấy userId từ token)
    /// </summary>
    /// <returns></returns>
    [HttpDelete("refresh-tokens/current")]
    [EndpointName("EmployeeRevokeRefreshTokensFromToken")]
    [Authorize]
    [ApiKey]
    public async Task<IResult> RevokeAllCurrentRefreshTokens()
    {
        string token = TokenExtentions.GetTokenFromHeader(HttpContext);
        var claims = TokenExtentions.DecodeJwt(token);
        claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        
        var result = await _sender.Send(new RevokeAllEmployeeRefreshTokensCommand(new Guid(sub!)));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
}
