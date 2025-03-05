using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class SignIn : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/customers/signin", async (
            [FromBody] CustomerSignInCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.SignIn)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
