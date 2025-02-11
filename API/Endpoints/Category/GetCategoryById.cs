namespace API.Endpoints.Category;

internal sealed class GetCategoryById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories/{id}", async () => 
        {
            
        });

    }
}
