using FluentValidation;

namespace Infrastructure.Options.Cloudinary;

internal sealed class CloudinaryOptionsValidator : AbstractValidator<CloudinaryOptions>
{
    public CloudinaryOptionsValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty()
            .WithErrorCode("CloudinaryApiKey.Required")
            .WithMessage("Cloudinary api key is required.");
    }
}
