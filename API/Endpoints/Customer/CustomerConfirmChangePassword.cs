using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerConfirmChangePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class CustomerConfirmChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/customers/{token:guid}/password-change-confirm", async (
            [FromRoute] Guid token,
            ISender sender) =>
        {
            var command = new CustomerConfirmChangePasswordCommand(token);
            var result = await sender.Send(command);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.ChangePasswordConfirm);
    }
}
