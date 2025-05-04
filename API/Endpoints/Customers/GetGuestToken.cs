using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.GenerateGuestToken;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class GetGuestToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/guest-token", async (
            ISender sender) =>
        {
            var result = await sender.Send(new GenerateGuestTokenCommand());
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithSummary("Lấy mã khách hàng")
        .WithDescription("""
            Cho phép khách hàng lấy mã khách hàng để sử dụng các dịch vụ của hệ thống.
            
            Sample Request:
            
                GET /api/customers/guest-token
            
            """)
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK);
    }
} 