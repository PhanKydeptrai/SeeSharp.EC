using Application.Abstractions.Messaging;
using Application.DTOs.Feedbacks;
using Application.IServices;
using Domain.Entities.Feedbacks;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.FeedbackFeature.Queries.GetFeedbackById;

public class GetFeedbackByIdQueryHandler : IQueryHandler<GetFeedbackByIdQuery, FeedbackResponse>
{
    private readonly IFeedbackQueryServices _feedbackQueryServices;

    public GetFeedbackByIdQueryHandler(IFeedbackQueryServices feedbackQueryServices)
    {
        _feedbackQueryServices = feedbackQueryServices;
    }

    public async Task<Result<FeedbackResponse>> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        var feedbackId = FeedbackId.FromGuid(request.FeedbackId);
        var feedbackResponse = await _feedbackQueryServices.GetFeedbackById(feedbackId);

        if (feedbackResponse is null)
        {
            return Result.Failure<FeedbackResponse>(FeedBackError.NotFound(feedbackId));
        }

        return Result.Success(feedbackResponse);
    }
}
