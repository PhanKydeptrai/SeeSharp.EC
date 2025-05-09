using FluentValidation;

namespace Infrastructure.Options.GituhubAuth;

internal sealed class GithubOptionsValidator : AbstractValidator<GithubOptions>
{
    public GithubOptionsValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithErrorCode("ClientId.Required")
            .WithMessage("ClientId is required.");

        RuleFor(x => x.ClientSecret)
            .NotEmpty()
            .WithErrorCode("ClientSecret.Required")
            .WithMessage("ClientSecret is required.");
    }
}
