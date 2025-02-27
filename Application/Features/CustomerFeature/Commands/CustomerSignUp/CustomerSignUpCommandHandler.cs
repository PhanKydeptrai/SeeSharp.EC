using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.Users;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignUp;

internal sealed class CustomerSignUpCommandHandler : ICommandHandler<CustomerSignUpCommand, CustomerSignUpResponse>
{
    #region Dependencies
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public CustomerSignUpCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IUserRepository userRepository)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _userRepository = userRepository;
    }
    #endregion

    public Task<Result<CustomerSignUpResponse>> Handle(
        CustomerSignUpCommand request,
        CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}
