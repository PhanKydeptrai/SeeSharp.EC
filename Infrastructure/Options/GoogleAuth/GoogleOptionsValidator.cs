using FluentValidation;

namespace Infrastructure.Options.Google;

internal sealed class GoogleOptionsValidator : AbstractValidator<GoogleOptions>
{
    public GoogleOptionsValidator()
    {
        RuleFor(x => x.ClientSecret)
            .NotEmpty()
            .WithErrorCode("GoogleClientSecret.Required")
            .WithMessage("ClientSecret is required.");

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithErrorCode("GoogleClientId.Required")
            .WithMessage("ClientId is required.");
    }
}
