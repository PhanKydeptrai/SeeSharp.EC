using Application.DTOs.Customer;
using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Application.IServices;

public interface ICustomerQueryServices
{
    Task<bool> IsCustomerEmailExist(
        CustomerId? customerId,
        Email email,
        CancellationToken cancellationToken = default);

    Task<CustomerAuthenticationResponse?> IsCustomerSignInSuccess(
        Email email, 
        PasswordHash password);


}
