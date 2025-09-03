using System.Data;
using FluentValidation;

namespace Application.Features.FeedbackFeature.Queries.GetFeecbackOfProduct;

internal sealed class GetFeedbackOfProductQueryValidator : AbstractValidator<GetFeedbackOfProductQuery>
{
    public GetFeedbackOfProductQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithErrorCode("ProductId.Required")
            .WithMessage("Product ID is required.");
    }
}
