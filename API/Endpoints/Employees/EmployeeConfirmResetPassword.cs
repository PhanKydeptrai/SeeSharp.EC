using Application.Features.EmployeeFeature.Commands.EmployeeConfirmResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Employees;

internal sealed class EmployeeConfirmResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/employees/confirm-reset-password/{verificationTokenId:guid}", async (
            [FromRoute] Guid verificationTokenId,
            ISender sender) =>
        {
            var result = await sender.Send(new EmployeeConfirmResetPasswordCommand(verificationTokenId));
            if (result.IsFailure)
            {
                return Results.Redirect("https://localhost:7259/password-reset-failed");
            }
            return Results.Redirect("https://localhost:7259/password-reset-success");
        })
        .WithTags(EndpointTags.Employee)
        .Produces(StatusCodes.Status302Found)
        .WithOpenApi(o => 
        {
            var verificationTokenIdParam = o.Parameters.FirstOrDefault(p => p.Name == "verificationTokenId");

            if (verificationTokenIdParam is not null)
            {
                verificationTokenIdParam.Description = "ID của mã xác thực (GUID)";
            }

            return o;
        })
        .WithSummary("Xác nhận yêu cầu đặt lại mật khẩu")
        .WithDescription("""
            Xác nhận yêu cầu đặt lại mật khẩu của nhân viên bằng mã xác thực được gửi qua email.
            
            Sample Request:
            
                GET /api/employees/confirm-reset-password/{verificationTokenId}
            
            """);
    }
} 