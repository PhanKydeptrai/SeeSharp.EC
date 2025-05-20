using Application.Features.CustomerFeature.Commands.CustomerSignInWithGithub;
using AspNet.Security.OAuth.GitHub;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using SharedKernel.Constants;
using System.Security.Claims;

namespace API.Endpoints.Customers;

internal sealed class ExternalGithubLoginCallBack : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customer/github-signin-callback", 
        async (
            ISender sender,
            HttpContext httpContext) =>
        {
            var authenticationResult = await httpContext.AuthenticateAsync(GitHubAuthenticationDefaults.AuthenticationScheme);

            if (!authenticationResult.Succeeded) return null;

            var claims = authenticationResult.Principal.Claims;


            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;

            var result = await sender.Send(new CustomerSignInWithGithubCommand(email, name));
            

            return Results.Redirect($"https://localhost:7222/api/customers/signin-github-callback?email={email}&name={name}");

        }).WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignInWithGithubCallback)
        .Produces(StatusCodes.Status302Found)
        .WithSummary("Call back url của github actions")
        .WithDescription("""
            Sample Request:
                GET /signin-github-callback
            """);

    }
}
