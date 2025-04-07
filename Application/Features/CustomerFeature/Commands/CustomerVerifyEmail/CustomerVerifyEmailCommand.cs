using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerVerifyEmail;

public record CustomerVerifyEmailCommand(Guid VerificationTokenId) : ICommand;
