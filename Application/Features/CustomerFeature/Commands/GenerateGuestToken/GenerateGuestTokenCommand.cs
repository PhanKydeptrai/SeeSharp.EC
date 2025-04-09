using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.GenerateGuestToken;

public record GenerateGuestTokenCommand() : ICommand<GuestTokenResponse>;

public record GuestTokenResponse(string Token);
