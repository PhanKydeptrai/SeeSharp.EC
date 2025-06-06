using Domain.Entities.Customers;
using Domain.Entities.Users;
using Domain.IRepositories.Customers;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.CustomerRepositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;
    public CustomerRepository(SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    public async Task AddCustomer(Customer customer)
    {
        await _postgreSQLWriteDbContext.Customers.AddAsync(customer);
    }

    public async Task<Customer?> GetCustomerByEmailFromPostgreSQL(Email email)
    {
        return await _postgreSQLWriteDbContext.Customers.Include(a => a.User).FirstOrDefaultAsync(a => a.User!.Email == email);
    }
    public async Task<Customer?> GetCustomerByUserId(UserId userId)
    {
        return await _postgreSQLWriteDbContext.Customers.Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<Customer?> GetCustomerByCustomerId(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.Customers.Include(a => a.User).FirstOrDefaultAsync(a => a.CustomerId == customerId);
    }
}
