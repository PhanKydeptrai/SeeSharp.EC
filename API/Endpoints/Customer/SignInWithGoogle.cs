using Application.Features.CustomerFeature.Commands.CustomerSignInWithGoogle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Customer;

internal sealed class SignInWithGoogle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/customers/sign-in/google/{token}", async (
            [FromRoute] string token,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerSignInWithGoogleCommand(token));

            
        });
    }
}
