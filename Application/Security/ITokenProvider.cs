using Domain.Entities.Customers;
using Domain.Entities.Employees;
using Domain.Entities.Users;

namespace Application.Security;

public interface ITokenProvider
{
    string GenerateAccessTokenForCustomer(UserId userId, CustomerId customerId, Email email, string role, string jti);
    string GenerateAccessTokenForEmployee(UserId userId, EmployeeId employeeId, Email email, string role, string jti);
    string GenerateRefreshToken();
    string GenerateRandomString(int length);
}