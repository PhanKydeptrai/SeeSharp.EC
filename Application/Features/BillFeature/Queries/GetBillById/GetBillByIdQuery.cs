using Application.Abstractions.Messaging;
using Application.DTOs.Bills;

namespace Application.Features.BillFeature.Queries.GetBillById;

public record GetBillByIdQuery(Guid BillId) : IQuery<BillResponse>;