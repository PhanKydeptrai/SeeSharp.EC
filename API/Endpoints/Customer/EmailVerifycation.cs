
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class EmailVerifycation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/customer/{token:guid}", async (
            Guid token) =>
        {
            return Task.CompletedTask;
        })
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.Verify);
    }
}
