using Domain.Entities.Customers;
using Domain.Entities.Users;
using Domain.IRepositories.Customers;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Customer?> GetCustomerByEmailFromMySQL(Email email)
    {
        return await _mySQLWriteDbContext.Customers
            .Where(a => a.User!.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetCustomerByEmailFromPostgreSQL(Email email)
    {
        return await _postgreSQLWriteDbContext.Customers
            .Where(a => a.User!.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetCustomerByFromMySQLByUserId(UserId userId)
    {
        return await _mySQLWriteDbContext.Customers.Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<Customer?> GetCustomerByFromPostgreSQLByUserId(UserId userId)
    {
        return await _postgreSQLWriteDbContext.Customers.Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }
}
