using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithFacebook;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class SignInWithFacebook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customer/facebook-signin", 
        async (
            CustomerSignInWithFacebookRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerSignInWithFacebookCommand(
                request.Email, 
                request.ImageUrl, 
                request.UserName));
            
            return result.Match(Results.Ok, CustomResults.Problem);

        }).WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignInWithFacebook)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .WithOpenApi()
        .WithSummary("Đăng nhập bằng Facebook")
        .WithDescription("""

            Cho phép khách hàng đăng nhập vào hệ thống bằng tài khoản Facebook.

            Sample Request:

                POST /api/customer/facebook-signin
                {
                    "email": "string",
                    "imageUrl": "string",
                    "userName": "string"
                }
            """);

    }

    private class CustomerSignInWithFacebookRequest
    {
        /// <summary>
        /// Email của khách hàng
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Đường dẫn đến ảnh đại diện của khách hàng
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Tên của khách hàng
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
}
