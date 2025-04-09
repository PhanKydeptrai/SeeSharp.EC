using Domain.Entities.Vouchers;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class VoucherError
{
    public static Error InvalidVoucherType(string voucherType) => Error.Problem(
        "Voucher.InvalidType",
        $"Voucher type '{voucherType}' is invalid. Must be 'Direct' or 'Percentage'.");
        
    public static Error NotFound(VoucherId voucherId) => Error.NotFound(
        "Voucher.NotFound",
        $"The voucher with the Id = '{voucherId}' was not found");
} 