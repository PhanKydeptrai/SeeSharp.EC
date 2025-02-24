using MediatR;

namespace API.Endpoints.Product;

internal sealed class UpdateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{productId:guid}", 
        async (
            ISender sender) =>
        {
            
        });
    }
}
