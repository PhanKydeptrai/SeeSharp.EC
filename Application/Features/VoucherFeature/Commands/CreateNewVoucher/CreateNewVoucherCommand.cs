using Application.Abstractions.Messaging;

namespace Application.Features.VoucherFeature.Commands.CreateNewVoucher;

public record CreateNewVoucherCommand(
    string VoucherName, 
    string VoucherCode,
    string VoucherType,
    int PercentageDiscount,
    decimal MaximumDiscountAmount,
    decimal MinimumOrderAmount,
    DateTime StartDate,
    DateTime ExpiredDate,
    string VoucherDescription) : ICommand;