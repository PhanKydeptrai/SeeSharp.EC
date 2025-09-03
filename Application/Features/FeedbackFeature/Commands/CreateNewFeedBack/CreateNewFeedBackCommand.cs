using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;

public record CreateNewFeedBackCommand(
    string Substance,
    int RatingScore,
    IFormFile? Image,
    bool IsPrivate,
    Guid OrderId,
    Guid CustomerId) : ICommand;
