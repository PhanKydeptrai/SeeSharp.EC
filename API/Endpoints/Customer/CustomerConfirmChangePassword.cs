using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Customer;

internal sealed class CustomerConfirmChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/customers/{token:guid}/password-change-confirm", async (
            [FromRoute] Guid token,
            ISender sender) =>
        {
            return Results.Ok();
        });
    }
}
