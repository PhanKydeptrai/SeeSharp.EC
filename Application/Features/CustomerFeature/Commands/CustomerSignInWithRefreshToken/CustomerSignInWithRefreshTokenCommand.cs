using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;

public record CustomerSignInWithRefreshTokenCommand(string RefreshToken) : ICommand<CustomerSignInResponse>;
