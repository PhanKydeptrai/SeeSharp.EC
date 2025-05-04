using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Categories;

internal sealed class GetCategoryById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories/{categoryId:guid}", async (
            [FromRoute] Guid categoryId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryByIdQuery(categoryId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.GetById)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .WithSummary("Lấy danh mục theo ID")
        .WithDescription("""
            Lấy danh mục theo ID.
            
            Sample Request:
            
                GET /api/categories/{categoryId}
            """)
        .WithOpenApi(operation =>
        {
            var categoryIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "categoryId");
            
            if (categoryIdParam is not null)
            {
                categoryIdParam.Description = "ID của danh mục (GUID)";
            }

            return operation;
        })
        .RequireAuthorization();
    }
}
