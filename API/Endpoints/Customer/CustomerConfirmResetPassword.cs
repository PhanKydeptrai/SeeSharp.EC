using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerConfirmResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

public class CustomerConfirmResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/customers/{token:guid}/reset-pass", async (
            [FromRoute] Guid token,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerConfirmResetPasswordCommand(token));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.ResetPasswordConfirm);
    }
}
