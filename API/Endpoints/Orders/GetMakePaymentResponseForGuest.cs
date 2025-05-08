using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetMakePaymentResponse;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class GetMakePaymentResponseForGuest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/make-payment/guest", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);

            var result = await sender.Send(new GetMakePaymentResponseQuery(new Guid(guestId!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithSummary("Lấy thông tin phản hồi thanh toán cho khách hàng chưa đăng nhập")
        .WithDescription("""
            Cho phép khách hàng chưa đăng nhập lấy thông tin phản hồi thanh toán của mình.
               
            Sample Request:
               
             GET /api/orders/make-payment/guest
               
            """)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithOpenApi();
    }
} 