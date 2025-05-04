using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.CreateNewEmployee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Employees;

internal sealed class CreateEmployee : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/employees", async (
            [FromForm] CreateEmployeeRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateNewEmployeeCommand(
                request.UserName,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth,
                request.ImageFile));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.Create)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Tạo nhân viên mới")
        .WithDescription("""
            Cho phép admin tạo một nhân viên mới.
            
            Sample Request:
            
                POST /api/employees
            
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class CreateEmployeeRequest
    {
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email nhân viên
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại nhân viên
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Ngày sinh nhân viên
        /// </summary>
        public DateOnly? DateOfBirth { get; set; }

        /// <summary>
        /// Ảnh đại diện nhân viên
        /// </summary>
        public IFormFile? ImageFile { get; set; }
    }
}