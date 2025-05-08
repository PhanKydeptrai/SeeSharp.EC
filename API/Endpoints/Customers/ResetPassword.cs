using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class ResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customers/reset-password", async (
            [FromBody] CustomerResetPasswordCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.ResetPassword)
        .WithSummary("Khôi phục mật khẩu")
        .WithDescription("""
            Cho phép khách hàng khôi phục mật khẩu của mình thông qua mã xác nhận được gửi qua email.
              
            Sample Request:
              
                POST /api/customers/reset-password
              
            """)
        .WithOpenApi();
    }
} 