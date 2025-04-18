using Application.Abstractions.Messaging;
using Application.DTOs.Bills;
using Application.IServices;
using Domain.Entities.Bills;
using SharedKernel;

namespace Application.Features.BillFeature.Queries.GetBillById;

internal sealed class GetBillByIdQueryHandler : IQueryHandler<GetBillByIdQuery, BillResponse>
{
    private readonly IOrderQueryServices _orderQueryServices;

    public GetBillByIdQueryHandler(IOrderQueryServices orderQueryServices)
    {
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result<BillResponse>> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
    {
        var billId = BillId.FromGuid(request.BillId);
        var result = await _orderQueryServices.GetBillByBillId(billId);

        if(result is null)
        {
            return Result.Failure<BillResponse>(new Error("Bill.NotFound", "Bill not found", ErrorType.NotFound));
        }

        return Result.Success(result);
    
    }
}
