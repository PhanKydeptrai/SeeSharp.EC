using API.Infrastructure;
using Application.Features.CategoryFeature.Queries.GetAllCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Category;

//internal sealed class GetAllCategory : IEndpoint
//{
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapGet("api/categories", async (
//            [FromQuery] string? filter,
//            [FromQuery] string? searchTerm,
//            [FromQuery] string? sortColumn,
//            [FromQuery] string? sortOrder,
//            [FromQuery] int? page,
//            [FromQuery] int? pageSize,
//            ISender sender) =>
//        {
//            var result = await sender.Send(
//                new GetAllCategoryQuery(
//                    filter, 
//                    searchTerm, 
//                    sortColumn, 
//                    sortOrder, 
//                    page, 
//                    pageSize));

//            return Results.Ok(result.Value);
//        })
//        .DisableAntiforgery()
//        .WithTags(EndpointTag.Category)
//        .WithName(EndpointName.Category.GetAll)
//        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
//    }
//}
