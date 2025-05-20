using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class SignInWithRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/refresh-token", 
        async (
            [FromBody] CustomerSignInWithRefreshTokenRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerSignInWithRefreshTokenCommand(
                request.RefreshToken));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignInWithRefreshToken)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Đăng nhập bằng mã xác thực")
        .WithDescription("""
            Cho phép khách hàng đăng nhập vào hệ thống bằng mã xác thực (refresh token).
              
            Sample Request:
              
                GET /api/customers/refresh-token
              
            """)
        .WithOpenApi();
    }

    private class CustomerSignInWithRefreshTokenRequest
    {
        /// <summary>
        /// Mã xác thực của khách hàng
        /// </summary>
        public string RefreshToken { get; init; } = string.Empty;
    }
} 