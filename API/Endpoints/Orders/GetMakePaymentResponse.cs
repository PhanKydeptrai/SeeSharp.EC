using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetMakePaymentResponse;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class GetMakePaymentResponse : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/make-payment", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var result = await sender.Send(new GetMakePaymentResponseQuery(new Guid(customerId!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.GetMakePaymentResponse)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin phản hồi thanh toán")
        .WithDescription("""
            Cho phép khách hàng lấy thông tin phản hồi thanh toán của mình.
               
            Sample Request:
               
             GET /api/orders/make-payment
               
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
} 