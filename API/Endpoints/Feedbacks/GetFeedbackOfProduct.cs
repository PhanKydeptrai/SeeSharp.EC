
using API.Extentions;
using API.Infrastructure;
using Application.Features.FeedbackFeature.Queries.GetFeecbackOfProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Feedbacks;

internal sealed class GetFeedbackOfProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/feedbacks/products/{productId:guid}", async (
            [FromRoute] Guid productId,
            [FromQuery] string? filter,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(new GetFeedbackOfProductQuery(
                productId,
                filter,
                sortColumn,
                sortOrder,
                page,
                pageSize));

            return result.Match(Results.Ok, CustomResults.Problem);
        }).WithTags(EndpointTags.Feedbacks)
        .WithSummary("Lấy danh sách phản hồi của sản phẩm")
        .WithDescription("""
            Lấy danh sách phản hồi của sản phẩm theo ID sản phẩm

            Sample Request:

                GET: /api/feedbacks/products/{productId}?filter={filter}&sortColumn={sortColumn}&sortOrder={sortOrder}&page={page}&pageSize={pageSize}

        """)
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .AddNotFoundResponse()
        .AddBadRequestResponse();
    }
}
