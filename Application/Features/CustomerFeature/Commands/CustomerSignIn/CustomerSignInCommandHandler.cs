using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Application.Security;
using Application.Services;
using CloudinaryDotNet.Actions;
using Domain.Entities.Customers;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CustomerEvents;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignIn;

internal sealed class CustomerSignInCommandHandler : ICommandHandler<CustomerSignInCommand, CustomerSignInResponse>
{
    #region Dependencies
    private readonly ICustomerQueryServices _customerQueryServices;
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerSupabaseClient _supabaseClient;
    public CustomerSignInCommandHandler(
        IUnitOfWork unitOfWork,
        ICustomerQueryServices customerQueryServices,
        ICustomerSupabaseClient supabaseClient,
        IVerificationTokenRepository verificationTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _customerQueryServices = customerQueryServices;
        _supabaseClient = supabaseClient;
        _verificationTokenRepository = verificationTokenRepository;
    }
    #endregion

    public async Task<Result<CustomerSignInResponse>> Handle(
        CustomerSignInCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _customerQueryServices.IsAccountVerified(Email.NewEmail(request.Email), cancellationToken))
        {
            //TODO: Gửi mail xác thực tài khoản
            // var verificationToken = VerificationToken.NewVerificationToken(string.Empty, , DateTime.UtcNow.AddDays(1));
            // var message = new SendVerificationEmailToCustomer(request.Email, Guid.NewGuid(), Ulid.NewUlid().ToGuid());
        }

        var session = await _supabaseClient.Client.Auth.SignIn(request.Email, request.Password);
        if (session is null) return Result.Failure<CustomerSignInResponse>(CustomerError.LoginFailed());
        return Result.Success(new CustomerSignInResponse(session.AccessToken!, session.RefreshToken!));
    }

    #region Private method
    // private async Task<(CustomerAuthenticationResponse? response, Result<CustomerSignInResponse>? failure)> IsSignInSuccess(
    //     CustomerSignInCommand request)
    // {
    //     // var response = await _customerQueryServices.AuthenticateCustomer(
    //     //     Email.NewEmail(request.Email),
    //     //     PasswordHash.NewPasswordHash(request.Password.SHA256()));

    //     try
    //     {
    //         var session = await _supabaseClient.Client.Auth.SignIn(request.Email, request.Password);


    //     }
    //     catch (Exception ex)
    //     {
    //         return (null, Result.Failure<CustomerSignInResponse>(CustomerError.LoginFailed()));
    //     }
    // }
    #endregion
}
