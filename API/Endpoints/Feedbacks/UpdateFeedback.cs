using API.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Feedbacks;
// Mỗi đánh giá được cập nhật 1 lần
public class UpdateFeedback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/feedbacks/{feedBackId}", 
        async (
            [FromRoute] Guid feedBackId,
            ISender sender) =>
        {
            var result = await sender.Send();
            return result.Match(Results.Ok, CustomResults.Problem);

        }).WithTags(EndpointTags.Feedbacks)
        .WithName(EndpointName.Feedback.Update)
        .WithSummary("Cập nhật đánh giá")
        .WithDescription("""
            Cập nhật đánh giá của khách hàng về đơn hàng.
            
            Sample Request:
            
                PUT: api/feedbacks/{feedBackId}
            """)
        .WithOpenApi(o =>
        {
            var billIdParam = o.Parameters.FirstOrDefault(p => p.Name == "feedBackId");

            if (billIdParam is not null)
            {
                billIdParam.Description = "ID của feedbacks (GUID)";
            }

            return o;
        });
    }
}
