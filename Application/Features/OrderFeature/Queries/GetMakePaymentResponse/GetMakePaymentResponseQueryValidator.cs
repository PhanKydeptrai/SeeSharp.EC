using FluentValidation;

namespace Application.Features.OrderFeature.Queries.GetMakePaymentResponse;

public sealed class GetMakePaymentResponseQueryValidator : AbstractValidator<GetMakePaymentResponseQuery>
{
    public GetMakePaymentResponseQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer Id is required");
    }
} 