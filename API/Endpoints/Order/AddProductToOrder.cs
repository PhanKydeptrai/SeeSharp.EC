
// using API.Extentions;
// using API.Infrastructure;
// using Application.Features.OrderFeature.Commands.AddProductToOrder;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using SharedKernel.Constants;

// namespace API.Endpoints.Order;

// internal sealed class AddProductToOrder : IEndpoint
// {
//     public void MapEndpoint(IEndpointRouteBuilder builder)
//     {
//         builder.MapPost("/api/orders", async (
//             [FromBody] AddProductToOrderRequest request,
//             HttpContext httpContext,
//             ISender sender) =>
//         {
//             string token = TokenExtentions.GetTokenFromHeader(httpContext);
//             var claims = TokenExtentions.DecodeJwt(token);
//             claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

//             var command = new AddProductToOrderCommand(request.ProductId, new Guid(customerId!), request.Quantity);
//             var result = await sender.Send(command);

//             return result.Match(Results.NoContent, CustomResults.Problem);
//         })
//         .DisableAntiforgery()
//         .WithTags(EndpointTag.Order)
//         .WithName(EndpointName.Order.AddProductToOrder)
//         .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
//         .RequireAuthorization();
//     }
// }
