using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerConfirmChangePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class ConfirmChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/confirm-change-password/{verificationTokenId:guid}", async (
            [FromRoute] Guid verificationTokenId,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerConfirmChangePasswordCommand(verificationTokenId));
            if (result.IsFailure)
            {
                return Results.Redirect("https://localhost:7115/change-password-success");
            }
            return Results.Redirect("https://localhost:7115/change-password-failed");
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.ChangePasswordConfirm)
        .Produces(StatusCodes.Status302Found)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Xác nhận thay đổi mật khẩu")
        .WithDescription("""
            Cho phép khách hàng xác nhận thay đổi mật khẩu của mình thông qua mã xác nhận được gửi qua email.
            
            Sample Request:
            
                GET /api/customers/confirm-change-password/{verificationTokenId}
            
            """)
        .WithOpenApi(o => 
        {
            var verificationTokenIdParam = o.Parameters.FirstOrDefault(p => p.Name == "verificationTokenId");

            if (verificationTokenIdParam != null)
            {
                verificationTokenIdParam.Description = "Mã xác nhận thay đổi mật khẩu";
                verificationTokenIdParam.Required = true;
            }
            
            return o;
        });
    }
} 