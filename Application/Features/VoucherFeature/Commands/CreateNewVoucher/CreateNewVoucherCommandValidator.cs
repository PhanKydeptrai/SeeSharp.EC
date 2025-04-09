using Domain.Entities.Vouchers;
using Domain.IRepositories.Vouchers;
using FluentValidation;

namespace Application.Features.VoucherFeature.Commands.CreateNewVoucher;

internal sealed class CreateNewVoucherCommandValidator : AbstractValidator<CreateNewVoucherCommand>
{
    public CreateNewVoucherCommandValidator(IVoucherRepository voucherRepository)
    {
        RuleFor(x => x.VoucherName)
            .NotEmpty()
            .WithErrorCode("VoucherName.IsRequired")
            .WithMessage("Voucher name is required")
            .MaximumLength(100)
            .WithErrorCode("VoucherName.TooLong")
            .WithMessage("Voucher name is too long")
            .MustAsync(async (name, _) =>
                !await voucherRepository.IsVoucherNameExist(VoucherName.FromString(name)))
            .WithErrorCode("VoucherName.NotUnique")
            .WithMessage("Voucher name already exists");

        RuleFor(x => x.VoucherCode)
            .NotEmpty()
            .WithErrorCode("VoucherCode.IsRequired")
            .WithMessage("Voucher code is required")
            .MaximumLength(50)
            .WithErrorCode("VoucherCode.TooLong")
            .WithMessage("Voucher code is too long")
            .MustAsync(async (code, _) =>
                !await voucherRepository.IsVoucherCodeExist(VoucherCode.FromString(code)))
            .WithErrorCode("VoucherCode.NotUnique")
            .WithMessage("Voucher code already exists");

        RuleFor(x => x.VoucherType)
            .NotEmpty()
            .WithErrorCode("VoucherType.IsRequired")
            .WithMessage("Voucher type is required")
            .Must(type => type.Equals("Direct", StringComparison.OrdinalIgnoreCase) || 
                         type.Equals("Percentage", StringComparison.OrdinalIgnoreCase))
            .WithErrorCode("VoucherType.Invalid")
            .WithMessage("Invalid voucher type. Must be 'Direct' or 'Percentage'");

        RuleFor(x => x.PercentageDiscount)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("PercentageDiscount.TooLow")
            .WithMessage("Percentage discount must be greater than or equal to 0")
            .LessThanOrEqualTo(100)
            .WithErrorCode("PercentageDiscount.TooHigh")
            .WithMessage("Percentage discount must be less than or equal to 100")
            .When(x => x.VoucherType.Equals("Percentage", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.MaximumDiscountAmount)
            .GreaterThan(0)
            .WithErrorCode("MaximumDiscountAmount.TooLow")
            .WithMessage("Maximum discount amount must be greater than 0");

        RuleFor(x => x.MinimumOrderAmount)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("MinimumOrderAmount.Invalid")
            .WithMessage("Minimum order amount must be greater than or equal to 0");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode("StartDate.IsRequired")
            .WithMessage("Start date is required");

        RuleFor(x => x.ExpiredDate)
            .NotEmpty()
            .WithErrorCode("ExpiredDate.IsRequired")
            .WithMessage("Expired date is required")
            .GreaterThan(x => x.StartDate)
            .WithErrorCode("ExpiredDate.MustBeAfterStartDate")
            .WithMessage("Expired date must be after start date");

        RuleFor(x => x.VoucherDescription)
            .MaximumLength(500)
            .WithErrorCode("VoucherDescription.TooLong")
            .WithMessage("Voucher description is too long");
    }
} 