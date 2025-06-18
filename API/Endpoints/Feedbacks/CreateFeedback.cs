using API.Extentions;
using API.Infrastructure;
using API.Infrastructure.Authorization;
using Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Feedbacks;

internal sealed class CreateFeedback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/feedbacks",
        async (
            [FromForm] CreateFeedbackRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
            if (customerId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateNewFeedBackCommand(
                request.Substance,
                request.RatingScore,
                request.Image,
                request.BillId,
                new Guid(customerId)));

            return result.Match(Results.Created, CustomResults.Problem);

        }).WithTags(EndpointTags.Feedbacks)
        .WithName(EndpointName.Feedback.Create)
        .Produces(StatusCodes.Status201Created)
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .DisableAntiforgery()
        .WithOpenApi()
        .WithSummary("Khách hàng đánh giá đơn hàng")
        .WithDescription("""
            Đánh giá sản phẩm của đơn hàng

            Sample Request:

                POST: /api/feedbacks
            """)
        .RequireAuthorization(AuthorizationPolicies.SubscribedOnly);

    }

    private class CreateFeedbackRequest
    {
        /// <summary>
        /// Nội dung đánh giá
        /// </summary>
        public string Substance { get; set; } = string.Empty;

        /// <summary>
        /// Điểm đánh giá
        /// </summary>
        public int RatingScore { get; set; }

        /// <summary>
        /// Hình ảnh đánh giá
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Mã hoá hàng
        /// </summary>
        public Guid BillId { get; set; }
    }
}
