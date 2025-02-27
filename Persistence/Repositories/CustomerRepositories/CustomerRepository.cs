using Domain.Entities.Customers;
using Domain.IRepositories.Customers;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.CustomerRepositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly NextSharpMySQLWriteDbContext _mySQLWriteDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;
    public CustomerRepository(
        NextSharpMySQLWriteDbContext mySQLWriteDbContext, 
        NextSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _mySQLWriteDbContext = mySQLWriteDbContext;
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    public async Task AddCustomerToMySQL(Customer customer)
    {
        await _mySQLWriteDbContext.Customers.AddAsync(customer);
    }

    public async Task AddCustomerToPostgreSQL(Customer customer)
    {
        await _postgreSQLWriteDbContext.Customers.AddAsync(customer);
    }

}
