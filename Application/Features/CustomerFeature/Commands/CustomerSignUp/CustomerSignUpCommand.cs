using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Commands.CustomerSignUp;

public record CustomerSignUpCommand(
    string Email, 
    string UserName, 
    string Password) : ICommand<CustomerSignUpResponse>;
