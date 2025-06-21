using Application.Abstractions.Messaging;
using Application.DTOs.Feedbacks;
using Application.Features.Pages;

namespace Application.Features.FeedbackFeature.Queries.GetFeecbackOfProduct;

public record GetFeedbackOfProductQuery(
    Guid ProductId,
    string? filter,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<FeedbackResponse>>;
