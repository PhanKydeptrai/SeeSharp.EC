using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.Entities.Vouchers;
using Domain.IRepositories;
using Domain.IRepositories.VerificationTokens;
using Domain.IRepositories.Vouchers;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CustomerEvents;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerVerifyEmail;

internal sealed class CustomerVerifyEmailCommandHandler : ICommandHandler<CustomerVerifyEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    public CustomerVerifyEmailCommandHandler(
        IUnitOfWork unitOfWork,
        IVerificationTokenRepository verificationTokenRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus,
        IVoucherRepository voucherRepository)
    {
        _unitOfWork = unitOfWork;
        _verificationTokenRepository = verificationTokenRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
        _voucherRepository = voucherRepository;
    }

    public async Task<Result> Handle(CustomerVerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var tokenId = VerificationTokenId.FromGuid(request.VerificationTokenId);
        var token = await _verificationTokenRepository.GetVerificationTokenFromPostgreSQL(tokenId);

        if (token is null || token.ExpiredDate < DateTime.UtcNow)
        {
            return Result.Failure(CustomerError.VerificationTokenInvalid(tokenId));
        }

        // Xóa token đã sử dụng

        token.User!.VerifyAccount();

        _verificationTokenRepository.RemoveVerificationTokenFrommPostgreSQL(token);

        //Tạo một customer voucher cho khách hàng mới
        var customerVoucher = CustomerVoucher.NewCustomerVoucher(
            VoucherId.FromGuid(new Guid("01966202-d308-ab67-f2a9-436e1dd0e91f")),
            token.User.Customer!.CustomerId,
            CustomerVoucherQuantity.FromInt(1));

        await _voucherRepository.AddCustomerVoucher(customerVoucher);


        var message = new CustomerConfirmedSuccessfullyEvent("NEWUSER01", token.User!.Email!.Value, Ulid.NewUlid().ToGuid());
        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);
        await _unitOfWork.SaveChangesAsync();
        
        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
}
