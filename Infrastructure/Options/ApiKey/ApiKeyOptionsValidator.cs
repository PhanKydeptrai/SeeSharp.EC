using FluentValidation;

namespace Infrastructure.Options.ApiKey;

internal sealed class ApiKeyOptionsValidator : AbstractValidator<ApiKeyOptions>
{
    public ApiKeyOptionsValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty()
            .WithErrorCode("ApiKey.IsEmpty")
            .WithMessage("Api key must not be empty");
    }
}
