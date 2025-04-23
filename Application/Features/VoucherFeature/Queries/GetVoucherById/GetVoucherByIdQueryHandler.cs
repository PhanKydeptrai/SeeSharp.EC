using Application.Abstractions.Messaging;
using Application.DTOs.Voucher;
using Application.IServices;
using Domain.Entities.Vouchers;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.VoucherFeature.Queries.GetVoucherById;

internal sealed class GetVoucherByIdQueryHandler : IQueryHandler<GetVoucherByIdQuery, VoucherResponse>
{
    private readonly IVoucherQueryServices _voucherQueryServices;

    public GetVoucherByIdQueryHandler(IVoucherQueryServices voucherQueryServices)
    {
        _voucherQueryServices = voucherQueryServices;
    }

    public async Task<Result<VoucherResponse>> Handle(
        GetVoucherByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var voucherId = VoucherId.FromGuid(request.VoucherId);
        var voucher = await _voucherQueryServices.GetVoucherById(voucherId, cancellationToken);

        if (voucher is null)
        {
            return Result.Failure<VoucherResponse>(VoucherError.NotFound(voucherId));
        }

        return Result.Success(voucher);
    }
} 