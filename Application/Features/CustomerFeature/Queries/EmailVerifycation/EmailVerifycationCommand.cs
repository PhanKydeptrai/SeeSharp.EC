using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Queries.EmailVerifycation;

public record EmailVerifycationCommand(Guid tokenId) : ICommand;
