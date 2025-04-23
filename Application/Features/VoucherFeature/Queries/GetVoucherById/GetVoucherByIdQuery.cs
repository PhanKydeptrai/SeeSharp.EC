using Application.Abstractions.Messaging;
using Application.DTOs.Voucher;

namespace Application.Features.VoucherFeature.Queries.GetVoucherById;

public record GetVoucherByIdQuery(Guid VoucherId) : IQuery<VoucherResponse>;
