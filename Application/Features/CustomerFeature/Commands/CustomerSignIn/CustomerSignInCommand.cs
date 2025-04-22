using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Commands.CustomerSignIn;

public record CustomerSignInCommand(
    string Email, 
    string Password) : ICommand<CustomerSignInResponse>;

public record CustomerSignInRequest(
    string Email, 
    string Password);