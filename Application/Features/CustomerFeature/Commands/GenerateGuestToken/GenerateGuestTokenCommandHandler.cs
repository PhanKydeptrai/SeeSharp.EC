using Application.Abstractions.Messaging;
using Application.Security;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.GenerateGuestToken;

internal sealed class GenerateGuestTokenCommandHandler : ICommandHandler<GenerateGuestTokenCommand, GuestTokenResponse>
{
    private readonly ITokenProvider _tokenProvider;
    public GenerateGuestTokenCommandHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<GuestTokenResponse>> Handle(
        GenerateGuestTokenCommand request, 
        CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GenerateAccessTokenForGuest();
        return Result.Success(new GuestTokenResponse(token));
    }
}
