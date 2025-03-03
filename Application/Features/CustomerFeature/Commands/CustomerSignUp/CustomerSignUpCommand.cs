using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerSignUp;

public record CustomerSignUpCommand(
    string Email, 
    string UserName, 
    string Password) : ICommand;
