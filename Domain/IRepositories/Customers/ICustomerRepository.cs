using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Domain.IRepositories.Customers;

public interface ICustomerRepository
{
    Task AddCustomerToMySQL(Customer customer);
    Task AddCustomerToPostgreSQL(Customer customer);
    Task<Customer?> GetCustomerByEmailFromMySQL(Email email);
    Task<Customer?> GetCustomerByEmailFromPostgreSQL(Email email);
    Task<Customer?> GetCustomerByFromMySQLByUserId(UserId userId);
    Task<Customer?> GetCustomerByFromPostgreSQLByUserId(UserId userId);
}
