using FluentValidation;

namespace Infrastructure.Options.Jwt;

internal sealed class JwtOptionsValidator : AbstractValidator<JwtOptions>
{
    public JwtOptionsValidator()
    {
        RuleFor(x => x.SecSigningKeyret)
            .NotEmpty()
            .WithErrorCode("JwtSigningKey.Required")
            .WithMessage("Jwt signing key is required.");

        RuleFor(x => x.Issuer)
            .NotEmpty()
            .WithErrorCode("JwtIssuer.Required")
            .WithMessage("Jwt issuer is required.");
        
        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithErrorCode("JwtAudience.Required")
            .WithMessage("Jwt audience is required.");
    }
}
