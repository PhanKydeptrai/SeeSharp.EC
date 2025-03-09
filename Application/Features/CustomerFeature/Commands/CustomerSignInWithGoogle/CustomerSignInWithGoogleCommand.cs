using Application.Abstractions.Messaging;
using Application.DTOs.Customer;


namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithGoogle;

public record CustomerSignInWithGoogleCommand(string token) : ICommand<CustomerSignInResponse>;