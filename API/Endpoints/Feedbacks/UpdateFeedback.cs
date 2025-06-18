using API.Extentions;
using API.Infrastructure;
using Application.Features.FeedbackFeature.Commands.UpdateFeedBack;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Feedbacks;
// Mỗi đánh giá được cập nhật 1 lần
public class UpdateFeedback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/feedbacks/{feedBackId}", 
        async (
            [FromRoute] Guid feedBackId,
            [FromForm] UpdateFeedbackRequest request,
            ISender sender,
            HttpContext httpContext) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var result = await sender.Send(
                new UpdateFeedBackCommmand(
                    feedBackId, 
                    request.RatingScore, 
                    request.Substance, 
                    new Guid(customerId!), 
                    request.Image));

            return result.Match(Results.NoContent, CustomResults.Problem);

        }).WithTags(EndpointTags.Feedbacks)
        .WithName(EndpointName.Feedback.Update)
        .Produces(StatusCodes.Status204NoContent)   
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .WithSummary("Cập nhật đánh giá")
        .WithDescription("""
            Cập nhật đánh giá của khách hàng về đơn hàng.
            
            Sample Request:
            
                PUT: /api/feedbacks/{feedBackId}
            """)
        .WithOpenApi(o =>
        {
            var billIdParam = o.Parameters.FirstOrDefault(p => p.Name == "feedBackId");

            if (billIdParam is not null)
            {
                billIdParam.Description = "ID của feedbacks (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }

    private class UpdateFeedbackRequest
    {
        /// <summary>
        /// Nội dung đánh giá
        /// </summary>
        public string Substance { get; set; } = string.Empty;
        /// <summary>
        /// Điểm đánh giá
        /// </summary>
        public float RatingScore { get; set; }
        /// <summary>
        /// Ảnh đánh giá
        /// </summary>
        public IFormFile? Image { get; set; }
    }

    //public record UpdateFeedBackCommmand(
    //    Guid FeedbackId,
    //    float RatingScore,
    //    string Substance,
    //    Guid CustomerId,
    //    IFormFile? Image) : ICommand;
}
