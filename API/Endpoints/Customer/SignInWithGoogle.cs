using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithGoogle;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using MediatR;

namespace API.Endpoints.Customer;

internal sealed class SignInWithGoogle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/customers/sign-in/google/{token}", async (
            [FromRoute] string token,
            ISender sender) =>
        {
            // var result = await sender.Send(new CustomerSignInWithGoogleCommand(token));
            // return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignInWithGoogle)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
