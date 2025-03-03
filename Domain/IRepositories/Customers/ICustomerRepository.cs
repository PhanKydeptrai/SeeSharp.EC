using Domain.Entities.Customers;

namespace Domain.IRepositories.Customers;

public interface ICustomerRepository
{
    Task AddCustomerToMySQL(Customer customer);
    Task AddCustomerToPostgreSQL(Customer customer);
}
