using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.FeedbackFeature.Commands.UpdateFeedBack;

public record UpdateFeedBackCommmand(
    Guid FeedbackId, 
    float RatingScore,
    string Substance,
    Guid CustomerId,
    IFormFile? Image) : ICommand;