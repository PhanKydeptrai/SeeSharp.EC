using Application.Abstractions.Messaging;
using Application.DTOs.Customer;


namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithExternal;

public record CustomerSignInWithGoogleCommand(string token) : ICommand<CustomerSignInResponse>;

//public record CustomerSignInWithGoogleCommand(
//    string UserName,
//    string Email) : ICommand<CustomerSignInResponse>;