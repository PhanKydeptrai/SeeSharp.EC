using API.Extentions;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class ExternalGithubLogin : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/signin-github", () =>
        {
            var properties = new AuthenticationProperties { RedirectUri = "https://localhost:7222/api/customer/github-signin-callback" };
            return Results.Challenge(properties, new[] { GitHubAuthenticationDefaults.AuthenticationScheme });
        }).WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.SignInWithGithub)
        .Produces(StatusCodes.Status302Found)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithOpenApi()
        .WithSummary("Đăng nhập bằng github")
        .WithDescription("""
            Cho phép người dùng đăng nhập vào hệ thống bằng tài khoản github của mình.
            
            Sample Request:
            
                GET /signin-github
            
            """)
        .DisableAntiforgery();
        
    }
}
