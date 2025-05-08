using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Queries.GetCustomerProfileById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class GetCustomerProfileById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/{userId:guid}", async (
            [FromRoute] Guid userId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerProfileByIdQuery(userId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.GetCustomerProfileById)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin cá nhân của khách hàng theo ID")
        .WithDescription("""
            Cho phép nhân viên lấy thông tin cá nhân của khách hàng theo ID.
              
            Sample Request:
              
                GET /api/customers/{userId}
              
            """)
        .WithOpenApi(o =>
        {
            var userIdParam = o.Parameters.FirstOrDefault(p => p.Name == "userId");

            if (userIdParam != null)
            {
                userIdParam.Description = "ID của khách hàng";
                userIdParam.Required = true;
            }
            
            return o;
        })
        .RequireAuthorization();
    }
} 