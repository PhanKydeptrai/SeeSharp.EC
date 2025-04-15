using Application.Abstractions.Messaging;
using Application.DTOs.Customer;


namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithExternal;

public record CustomerSignInWithExternalCommand(
    string UserName,
    string Email) : ICommand<CustomerSignInResponse>;