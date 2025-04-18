using API.Documents;
using API.Infrastructure;
using Application.Features.BillFeature.Queries.GetBillById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace API.Controllers;

[Route("api/bills")]
[ApiController]
public class BillsController : ControllerBase
{
    private readonly ISender _sender;

    public BillsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{billId:guid}")]
    public async Task<IResult> GetBillById([FromRoute] Guid billId)
    {
        var result = await _sender.Send(new GetBillByIdQuery(billId));
        if(result.IsFailure)
        {
            return CustomResults.Problem(result);
        }

        // Generate PDF document
        var document = new BillDocument(result.Value);
        var pdfDocument = document.GeneratePdf();

        return Results.File(pdfDocument, "application/pdf", $"bill-{billId}.pdf");
    }
}
