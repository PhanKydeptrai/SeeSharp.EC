namespace API.Endpoints.Category;

//internal sealed class GetCategoryById : IEndpoint
//{   
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapGet("api/categories/{categoryId:guid}", async (
//            [FromRoute] Guid categoryId,
//            ISender sender) =>
//        {
//            var result = await sender.Send(new GetCategoryByIdQuery(categoryId));
//            return result.Match(Results.Ok, CustomResults.Problem);
//        })
//        .DisableAntiforgery()
//        .WithTags(EndpointTag.Category)
//        .WithName(EndpointName.Category.GetById)
//        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();

//    }
//}
