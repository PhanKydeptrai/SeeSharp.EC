using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class SignIn : IEndpoint
{
    // public void MapEndpoint(IEndpointRouteBuilder builder)
    // {
    //     builder.MapPost("api/customers/signin", async (
    //         [FromBody] CustomerSignInCommand command,
    //         ISender sender) =>
    //     {
    //         var result = await sender.Send(command);
    //         return result.Match(Results.Ok, CustomResults.Problem);
    //     })
    //     .DisableAntiforgery()
    //     .WithTags(EndpointTag.Customer)
    //     .WithName(EndpointName.Customer.SignIn)
    //     .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    // }

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/customers/signin", async (
            [FromBody] CustomerSignInCommand command,
            HttpContext httpContext,
            ISender sender) =>
        {
            var result = await sender.Send(command);

            httpContext.Response.Cookies.Append("access_token", result.Value.accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(30)
            });

            httpContext.Response.Cookies.Append("refresh_token", result.Value.refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(7) 
            });

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.SignIn)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
