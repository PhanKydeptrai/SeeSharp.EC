using Application.Abstractions.Messaging;
using Application.DTOs.Feedbacks;

namespace Application.Features.FeedbackFeature.Queries.GetFeedbackById;

public record GetFeedbackByIdQuery(Guid FeedbackId) : IQuery<FeedbackResponse>;