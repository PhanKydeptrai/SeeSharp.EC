using Application.DTOs.Customer;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.CustomerServices;

internal sealed class CustomerQueryServices : ICustomerQueryServices
{
    private readonly NextSharpPostgreSQLReadDbContext _dbContext;
    public CustomerQueryServices(
        NextSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsCustomerEmailExist(
        CustomerId? customerId, 
        Email email, 
        CancellationToken cancellationToken = default)
    {
        if (customerId is not null)
        {
            return await _dbContext.Customers.AnyAsync(
                a => a.CustomerId == new Ulid(customerId.Value) 
                    && a.UserReadModel.Email == email.Value, cancellationToken);
        }

        return await _dbContext.Customers.AnyAsync(
                a => a.UserReadModel.Email == email.Value, cancellationToken);
        
    }

    public async Task<CustomerAuthenticationResponse?> AuthenticateCustomer(
        Email email, PasswordHash password)
    {
        return await _dbContext.Customers.Where(
            a => a.UserReadModel.Email == email.Value 
            && a.UserReadModel.PasswordHash == password.Value
            && a.UserReadModel.IsVerify == true
            && a.UserReadModel.UserStatus != (int)UserStatus.Deleted
            && a.UserReadModel.UserStatus != (int)UserStatus.Blocked)
            .Select(a => new CustomerAuthenticationResponse(
                a.UserReadModel.UserId,
                a.UserReadModel.Email,
                a.UserReadModel.UserStatus.ToString(),
                a.CustomerType.ToString()))
            .FirstOrDefaultAsync();
    }
    public async Task<CustomerAuthenticationResponse?> GetCustomerByEmail(Email email)
    {
        return await _dbContext.Customers.Where(
            a => a.UserReadModel.Email == email.Value 
            && a.UserReadModel.UserStatus != (int)UserStatus.Deleted
            && a.UserReadModel.UserStatus != (int)UserStatus.Blocked)
            .Select(a => new CustomerAuthenticationResponse(
                a.UserReadModel.UserId,
                a.UserReadModel.Email,
                a.UserReadModel.UserStatus.ToString(),
                a.CustomerType.ToString()))
            .FirstOrDefaultAsync();
    }
}
