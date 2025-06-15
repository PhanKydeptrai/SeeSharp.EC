using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignUp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class SignUp : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customers/signup", async (
            [FromBody] CustomerSignUpRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerSignUpCommand(
                request.Email,
                request.UserName,
                request.Password));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignUp)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Đăng ký tài khoản")
        .WithDescription("""
            Cho phép khách hàng đăng ký tài khoản mới.
             
            Sample Request:
             
                POST /api/customers/signup
                {
                    "name": "Nguyen Van A",
                    "email": "nguyenvana@gmail.com"
                    "password": "123456789"
                }
            """)
        .WithOpenApi();
    }

    private class CustomerSignUpRequest
    {
        /// <summary>
        /// Tên của khách hàng
        /// </summary>
        public string UserName { get; init; } = string.Empty;

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