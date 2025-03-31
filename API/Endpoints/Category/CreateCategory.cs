namespace API.Endpoints.Category;

//internal sealed class CreateCategory : IEndpoint
//{
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapPost("api/categories", async (
//            [FromForm] string categoryName,
//            [FromForm] IFormFile? image,
//            ISender sender) =>
//        {
//            var command = new CreateCategoryCommand(categoryName, image);
//            var result = await sender.Send(command);
//            return result.Match(Results.Ok, CustomResults.Problem);
//        })
//        .DisableAntiforgery()
//        .WithSummary("Create a new category")
//        .WithTags(EndpointTag.Category)
//        .WithName(EndpointName.Category.Create)
//        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
//        .RequireAuthorization();
//    }
//}
