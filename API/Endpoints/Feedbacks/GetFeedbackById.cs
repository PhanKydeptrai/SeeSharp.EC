using API.Extentions;
using API.Infrastructure;
using Application.DTOs.Feedbacks;
using Application.Features.FeedbackFeature.Queries.GetFeedbackById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Feedbacks;

public class GetFeedbackById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/feedbacks/{feedbackId:guid}", async (
            [FromRoute] Guid feedbackId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetFeedbackByIdQuery(feedbackId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Feedbacks)
        .WithName(EndpointName.Feedback.GetFeedbackById)
        .WithSummary("Lấy thông tin phản hồi theo ID")
        .WithDescription("""
            Lấy thông tin phản hồi theo ID

            Sample Request:
                
                GET: /api/feedbacks/{feedbackId}
            
        """)
        .Produces<FeedbackResponse>(StatusCodes.Status200OK)
        .AddNotFoundResponse()
        .Produces(StatusCodes.Status404NotFound);
    }
}
