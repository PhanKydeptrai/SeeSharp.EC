using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class CustomerResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("api/customers/password/reset", async (
            [FromBody] CustomerResetPasswordCommand command,
            ISender sender) =>
        {   
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.ResetPassword)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
