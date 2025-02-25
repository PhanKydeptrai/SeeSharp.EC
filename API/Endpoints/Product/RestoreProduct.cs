using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Product;

internal sealed class RestoreProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {

        app.MapPut("api/products/{productId:guid}/restore", async (
            [FromRoute] Guid productId,
            ISender sender) =>
        {
            
        });
    }
}
