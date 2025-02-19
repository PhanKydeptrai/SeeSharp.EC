using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Category;

internal sealed class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/categories/{categoryId}", async (
            [FromRoute] string categoryId,
            [FromForm] string categoryName,
            [FromForm] IFormFile? image,
            ISender sender) =>
        {
            
        });
    }
}