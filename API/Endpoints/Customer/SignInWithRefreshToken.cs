
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class SignInWithRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/customers/refresh-token", async (
            [FromBody] CustomerSignInWithRefreshTokenCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.SignInWithRefreshToken);
    }
}
//NOTE:
/**/