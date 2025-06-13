using API.Documents;
using API.Infrastructure;
using Application.Features.BillFeature.Queries.GetBillById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using SharedKernel.Constants;

namespace API.Endpoints.Bills;

internal sealed class GetBillById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/bills/{billId:guid}", 
        async (
            [FromRoute] Guid billId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetBillByIdQuery(billId));
            if(result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            // Generate PDF document
            var document = new BillDocument(result.Value);
            var pdfDocument = document.GeneratePdf();

            return Results.File(pdfDocument, "application/pdf", $"bill-{billId}.pdf");
        })
        .WithTags(EndpointTags.Bills)
        .WithName(EndpointName.Bill.GetById)
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Lấy hóa đơn theo ID")
        .WithDescription("""
            Lấy hóa đơn theo ID và trả về dưới dạng tệp PDF.
               
            Sample Request:
               
                GET /api/bills/{billId}
               
            """)
        .WithOpenApi(o =>
        {
            var billIdParam = o.Parameters.FirstOrDefault(p => p.Name == "billId");

            if (billIdParam is not null)
            {
                billIdParam.Description = "ID của hóa đơn (GUID)";
            }

            return o;
        });
    }
} 