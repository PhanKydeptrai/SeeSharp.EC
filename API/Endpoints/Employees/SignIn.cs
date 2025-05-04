using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.EmployeeSignIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Employees;

internal sealed class SignIn : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/employees/sign-in", async (
            [FromBody] EmployeeSignInRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new EmployeeSignInCommand(
                request.Email,
                request.Password));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.SignIn)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Nhân viên đăng nhập")
        .WithDescription("""
            Cho phép nhân viên đăng nhập vào hệ thống.

            Sample Request:

                POST /api/employees/sign-in
                {
                    "email": "kyp12312@gmail.com"
                    "password": "123456789"
                }

            """);
    }
    private class EmployeeSignInRequest
    {
        /// <summary>
        /// Tên đăng nhập của nhân viên
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu của nhân viên
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
} 