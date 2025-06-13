using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithFacebook;

public record CustomerSignInWithFacebookCommand(
    string email,
    string imageUrl,
    string userName) : ICommand<CustomerSignInResponse>;
