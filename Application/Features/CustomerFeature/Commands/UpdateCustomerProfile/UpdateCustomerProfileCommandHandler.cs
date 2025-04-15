using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.UpdateCustomerProfile;

internal sealed class UpdateCustomerProfileCommandHandler : ICommandHandler<UpdateCustomerProfileCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCustomerProfileCommandHandler(
        ICustomerRepository customerRepository, 
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.FromGuid(request.UserId);
        var customer = await _customerRepository.GetCustomerByUserId(userId);

        if(customer is null)    
        {
            return Result.Failure(CustomerError.NotFoundCustomer());
        }

        customer.User!.UpdateUser(
            UserName.FromString(request.UserName),
            PhoneNumber.FromString(request.PhoneNumber),
            request.DateOfBirth,
            (Gender)request.Gender,
            string.Empty);
        
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
