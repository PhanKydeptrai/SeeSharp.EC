using Application.Abstractions.Messaging;

namespace Application.Features.VoucherFeature.Commands.CreateNewVoucher;

public record CreateNewVoucherCommand(
    string VoucherName, 
    string VoucherCode,
    string VoucherType,
    int PercentageDiscount,
    decimal MaximumDiscountAmount,
    decimal MinimumOrderAmount,
    DateOnly StartDate,
    DateOnly ExpiredDate,
    string VoucherDescription) : ICommand;