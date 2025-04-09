using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Domain.IRepositories.Customers;

public interface ICustomerRepository
{
    Task AddCustomer(Customer customer);
    Task<Customer?> GetCustomerByEmailFromPostgreSQL(Email email);
    Task<Customer?> GetCustomerByFromPostgreSQLByUserId(UserId userId);
}
