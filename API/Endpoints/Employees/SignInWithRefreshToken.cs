using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.EmployeeSignInWithRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Employees;

internal sealed class SignInWithRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/employees/refresh-token", async (
            [FromBody] EmployeeSignInWithRefreshTokenRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new EmployeeSignInWithRefreshTokenCommand(request.RefreshToken));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.SignInWithRefreshToken)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Nhân viên đăng nhập với mã thông báo làm mới")
        .WithDescription("""
            Cho phép nhân viên đăng nhập vào hệ thống với mã thông báo làm mới.
            
            Sample Request:
            
                POST /api/employees/refresh-token
                {
                    "refreshToken": "example-token"
                }
            
            """)
            .WithOpenApi();
    }

    private class EmployeeSignInWithRefreshTokenRequest
    {
        /// <summary>
        /// Mã thông báo làm mới của nhân viên
        /// </summary>
        public string RefreshToken { get; init; } = string.Empty;
    }
} 