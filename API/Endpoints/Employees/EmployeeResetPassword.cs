using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.EmployeeResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Employees;

internal sealed class EmployeeResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/employees/reset-password", async (
            [FromBody] EmployeeResetPasswordRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new EmployeeResetPasswordCommand(request.Email));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.ResetPassword)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .WithSummary("Khôi phục mật khẩu nhân viên")
        .WithDescription("""
            Cho phép nhân viên khôi phục mật khẩu của mình thông qua email.
             
            Sample Request:
             
                POST /api/employees/reset-password
                {
                    "email": "employee@gmail.com"
                }
                
            """);
    }

    private class EmployeeResetPasswordRequest
    {
        /// <summary>
        /// Email nhân viên
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
}