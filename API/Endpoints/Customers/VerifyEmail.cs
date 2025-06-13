using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerVerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class VerifyEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/verify-email/{verificationTokenId:guid}", 
        async (
            [FromRoute] Guid verificationTokenId,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerVerifyEmailCommand(verificationTokenId));
            if (result.IsFailure)
            {
                return Results.Redirect("https://localhost:7115/authentication-failed");
            }
            return Results.Redirect("https://localhost:7115/authentication-success");
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.Verify)
        .WithSummary("Xác thực email")
        .WithDescription("""
            Cho phép khách hàng xác thực email của mình thông qua mã xác nhận được gửi qua email.
              
            Sample Request:
              
                GET /api/customers/verify-email/{verificationTokenId}
              
            """)
        .WithOpenApi(o => 
        {
            var verificationTokenIdParam = o.Parameters.FirstOrDefault(p => p.Name == "verificationTokenId");

            if (verificationTokenIdParam != null)
            {
                verificationTokenIdParam.Description = "Mã xác nhận xác thực email";
                verificationTokenIdParam.Required = true;
            }
            
            return o;
        });
    }
} 