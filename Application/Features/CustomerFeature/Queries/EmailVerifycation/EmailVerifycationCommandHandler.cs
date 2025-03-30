// using Application.Abstractions.EventBus;
// using Application.Abstractions.Messaging;
// using Application.Outbox;
// using Domain.Entities.VerificationTokens;
// using Domain.IRepositories;
// using Domain.IRepositories.VerificationTokens;
// using Domain.OutboxMessages.Services;
// using Domain.Utilities.Errors;
// using Domain.Utilities.Events.CustomerEvents;
// using Microsoft.AspNetCore.Http.Features;
// using SharedKernel;

// namespace Application.Features.CustomerFeature.Queries.EmailVerifycation;

// internal sealed class EmailVerificationCommandHandler : ICommandHandler<EmailVerifycationCommand>
// {
//     #region Dependencies
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IEventBus _eventBus;
//     private readonly IVerificationTokenRepository _verificationTokenRepository;
//     private readonly IOutBoxMessageServices _outBoxMessageServices;
//     public EmailVerificationCommandHandler(
//         IUnitOfWork unitOfWork,
//         IVerificationTokenRepository verificationTokenRepository,
//         IEventBus eventBus,
//         IOutBoxMessageServices outBoxMessageServices)
//     {
//         _unitOfWork = unitOfWork;
//         _verificationTokenRepository = verificationTokenRepository;
//         _eventBus = eventBus;
//         _outBoxMessageServices = outBoxMessageServices;
//     }
//     #endregion

//     public async Task<Result> Handle(EmailVerifycationCommand request, CancellationToken cancellationToken)
//     {
//         var (verificationToken, failure) = await GetVerificationToken(request);

//         if(verificationToken is null) return failure!;
        
//         verificationToken.User!.VerifyAccount();

//         _verificationTokenRepository.RemoveVerificationTokenFromMySQL(verificationToken);
        
//         var message = new CustomerVerifiedEmailEvent(verificationToken.UserId.Value, Ulid.NewUlid().ToGuid());
        
//         await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);

//         await _unitOfWork.SaveChangeAsync();
        
//         await _eventBus.PublishAsync(message);
        
//         return Result.Success();
//     }

//     private async Task<(VerificationToken? token, Result? result)> GetVerificationToken(EmailVerifycationCommand request)
//     {
//         var tokenId = VerificationTokenId.FromGuid(request.tokenId);

//         var verificationToken = await _verificationTokenRepository.GetVerificationTokenFromMySQL(tokenId);

        
//         if(verificationToken is null || verificationToken.ExpiredDate < DateTime.UtcNow)
//         {
//             return (null, Result.Failure(CustomerError.NotFoundToken(tokenId)));
//         }

//         return (verificationToken, null);
//     }
// }
