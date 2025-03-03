
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Queries.EmailVerifycation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class EmailVerification : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/email-verifications/{token:guid}", async (
            [FromRoute] Guid token,
            ISender sender) =>
        {
            var result = await sender.Send(new EmailVerifycationCommand(token));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.Verify);
    }
}
