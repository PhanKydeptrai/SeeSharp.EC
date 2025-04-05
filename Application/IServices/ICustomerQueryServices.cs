using Application.DTOs.Customer;
using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Application.IServices;

public interface ICustomerQueryServices
{
    Task<bool> IsAccountVerified(
        Email email, 
        CancellationToken cancellationToken = default);
    Task<bool> IsCustomerEmailExist(
        CustomerId? customerId,
        Email email,
        CancellationToken cancellationToken = default);
    Task<CustomerAuthenticationResponse?> AuthenticateCustomer(
        Email email,
        PasswordHash password);

    Task<CustomerAuthenticationResponse?> GetCustomerByEmail(Email email);

    Task<bool> IsCustomerAccountExist(Email email, CancellationToken cancellationToken = default);
    Task<CustomerProfileResponse?> GetCustomerProfileById(UserId userId);
    
}
