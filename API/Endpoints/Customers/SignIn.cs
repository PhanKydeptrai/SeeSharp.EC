using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class SignIn : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customers/signin", 
        async (
            [FromBody] CustomerSignInRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerSignInCommand(
                request.Email,
                request.Password));

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignIn)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Đăng nhập")
        .WithDescription("""
            Cho phép khách hàng đăng nhập vào hệ thống.
             
            Sample Request:
             
                POST /api/customers/signin
             
            """)
        .WithOpenApi();
    }

    private class CustomerSignInRequest
    {
        /// <summary>
        /// Email của khách hàng
        /// </summary>
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Mật khẩu của khách hàng
        /// </summary>
        public string Password { get; init; } = string.Empty;
    }
}