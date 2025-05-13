using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerSignInWithExternal;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class SignInWithGoogle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("google-signin/{token}", 
        async (
            string token, 
            ISender sender) =>
        {
            var result = await sender.Send(new CustomerSignInWithGoogleCommand(token));
            return result.Match(Results.Ok, CustomResults.Problem);
        }).WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignInWithGoogle)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .WithSummary("Đăng nhập bằng Google")
        .WithDescription("""

            Cho phép khách hàng đăng nhập vào hệ thống bằng tài khoản Google.

            Sample Request:

                POST /google-signin/{token}

            """)
        .WithOpenApi(o =>
        {
            var tokenParam = o.Parameters.FirstOrDefault(p => p.Name == "verificationTokenId");

            if (tokenParam is not null)
            {
                tokenParam.Description = "Mã xác thực của tài khoản Google";
            }

            return o;
        });

    }
}
