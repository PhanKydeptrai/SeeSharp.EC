using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithGithub;

public record CustomerSignInWithGithubCommand(string Email, string UserName) : ICommand<CustomerSignInResponse>;
