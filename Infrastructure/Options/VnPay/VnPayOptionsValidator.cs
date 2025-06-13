using FluentValidation;

namespace Infrastructure.Options.VnPay;

internal sealed class VnPayOptionsValidator : AbstractValidator<VnPayOptions>
{
    public VnPayOptionsValidator()
    {
        RuleFor(x => x.VNP_URL)
            .NotEmpty()
            .WithErrorCode("VnPayUrl.Required")
            .WithMessage("VnPay URL is required.");

        RuleFor(x => x.VNP_TMN_CODE)
            .NotEmpty()
            .WithErrorCode("VnPayTmnCode.Required")
            .WithMessage("VnPay TMN Code is required.");

        RuleFor(x => x.VNP_RETURNURL_ORDERS)
            .NotEmpty()
            .WithErrorCode("VnPayReturnUrlOrders.Required")
            .WithMessage("VnPay Return URL for Orders is required.");

        RuleFor(x => x.VNP_HASH_SECRET)
            .NotEmpty()
            .WithErrorCode("VnPayHashSecret.Required")
            .WithMessage("VnPay Hash Secret is required.");
    }
}   
