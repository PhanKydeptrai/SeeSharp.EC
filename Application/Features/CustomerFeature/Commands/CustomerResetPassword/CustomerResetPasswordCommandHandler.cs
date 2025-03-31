// using Application.Abstractions.EventBus;
// using Application.Abstractions.Messaging;
// using Application.IServices;
// using Application.Outbox;
// using Application.Security;
// using Domain.Entities.Users;
// using Domain.Entities.VerificationTokens;
// using Domain.IRepositories;
// using Domain.IRepositories.VerificationTokens;
// using Domain.OutboxMessages.Services;
// using Domain.Utilities.Errors;
// using Domain.Utilities.Events.CustomerEvents;
// using SharedKernel;

// namespace Application.Features.CustomerFeature.Commands.CustomerResetPassword;

// internal sealed class CustomerResetPasswordCommandHandler : ICommandHandler<CustomerResetPasswordCommand>
// {
//     private readonly IVerificationTokenRepository _verificationTokenRepository;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly ICustomerQueryServices _customerQueryServices;
//     private readonly IOutBoxMessageServices _outBoxMessageServices;
//     private readonly ITokenProvider _tokenProvider;
//     private readonly IEventBus _eventBus;
//     public CustomerResetPasswordCommandHandler(
//         IVerificationTokenRepository verificationTokenRepository,
//         IUnitOfWork unitOfWork,
//         IOutBoxMessageServices outBoxMessageServices,
//         IEventBus eventBus,
//         ICustomerQueryServices customerQueryServices,
//         ITokenProvider tokenProvider)
//     {
//         ;
//         _verificationTokenRepository = verificationTokenRepository;
//         _unitOfWork = unitOfWork;
//         _outBoxMessageServices = outBoxMessageServices;
//         _eventBus = eventBus;
//         _customerQueryServices = customerQueryServices;
//         _tokenProvider = tokenProvider;
//     }

//     public async Task<Result> Handle(CustomerResetPasswordCommand request, CancellationToken cancellationToken)
//     {
//         //Check email is valid
//         var customer = await _customerQueryServices.GetCustomerByEmail(Email.NewEmail(request.Email));
//         if(customer is null) return Result.Failure(CustomerError.InValidInformation());
        
//         var token = VerificationToken.NewVerificationToken(
//             string.Empty, 
//             UserId.FromUlid(customer.UserId), 
//             DateTime.UtcNow.AddMinutes(15));

//         await _verificationTokenRepository.AddVerificationTokenToMySQL(token);
    
//         var message = new CustomerResetPasswordEmailSendEvent(
//             customer.UserId.ToGuid(),
//             token.VerificationTokenId,
//             customer.Email,
//             Ulid.NewUlid().ToGuid());

//         await OutboxMessageExtentions.InsertOutboxMessageAsync(
//             message.MessageId, message, 
//             _outBoxMessageServices);
        
//         await _unitOfWork.SaveChangeAsync();
//         await _eventBus.PublishAsync(message);
//         return Result.Success();
//     }
// }
