using Application.DTOs.Customer;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Customers;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;
using System.Linq.Expressions;

namespace Infrastructure.Services.CustomerServices;

internal sealed class CustomerQueryServices : ICustomerQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;
    public CustomerQueryServices(
        SeeSharpPostgreSQLReadDbContext dbContext)
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

    public async Task<bool> IsCustomerAccountExist(
        Email email, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers.AnyAsync(
                a => a.UserReadModel.Email == email.Value 
                && a.UserReadModel.IsVerify == true,
                cancellationToken);   
    }

    public async Task<CustomerAuthenticationResponse?> AuthenticateCustomer(
        Email email, PasswordHash password)
    {
        return await _dbContext.Customers.Where(
            a => a.UserReadModel.Email == email.Value 
            && a.UserReadModel.PasswordHash == password.Value
            && a.UserReadModel.IsVerify == true
            && a.UserReadModel.UserStatus != UserStatus.Deleted
            && a.UserReadModel.UserStatus != UserStatus.Blocked)
            .Select(a => new CustomerAuthenticationResponse(
                a.UserReadModel.UserId,
                a.CustomerId,
                a.UserReadModel.Email,
                a.UserReadModel.UserStatus.ToString(),
                a.CustomerType.ToString()))
            .FirstOrDefaultAsync();
    }
    public async Task<CustomerAuthenticationResponse?> GetCustomerByEmail(Email email)
    {
        return await _dbContext.Customers.Where(
            a => a.UserReadModel.Email == email.Value 
            && a.UserReadModel.UserStatus != UserStatus.Deleted
            && a.UserReadModel.UserStatus != UserStatus.Blocked)
            .Select(a => new CustomerAuthenticationResponse(
                a.UserReadModel.UserId,
                a.CustomerId,
                a.UserReadModel.Email,
                a.UserReadModel.UserStatus.ToString(),
                a.CustomerType.ToString()))
            .FirstOrDefaultAsync();
    }

    public async Task<CustomerProfileResponse?> GetCustomerProfileById(UserId userId)
    {
        return await _dbContext.Customers
            .Where(a => a.UserId == new Ulid(userId.Value))
            .Select(a => new CustomerProfileResponse(
                a.UserId.ToGuid(),
                a.UserReadModel.UserName,
                a.UserReadModel.DateOfBirth,
                a.UserReadModel.ImageUrl!,
                a.UserReadModel.Gender.ToString(),
                a.UserReadModel.PhoneNumber,
                a.UserReadModel.Email,
                a.CustomerType.ToString(),
                a.UserReadModel.UserStatus.ToString()))
            .FirstOrDefaultAsync();
    }

    public async Task<PagedList<CustomerProfileResponse>> GetAllCustomerProfile(
        string? statusFilter,
        string? customerTypeFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var customerQuery = _dbContext.Customers
            .Include(a => a.UserReadModel)
            .AsQueryable();

        //Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            customerQuery = customerQuery.Where(
                x => x.UserReadModel.UserName.Contains(searchTerm));
        }

        //Filter
        if (!string.IsNullOrEmpty(statusFilter))
        {
            customerQuery = customerQuery
                .Where(
                    x => x.UserReadModel.UserStatus == (UserStatus)Enum.Parse(typeof(UserStatus), 
                    statusFilter));
        }

        if (!string.IsNullOrEmpty(customerTypeFilter))
        {
            customerQuery = customerQuery
                .Where(
                    x => x.CustomerType == (CustomerType)Enum.Parse(typeof(CustomerType),
                    customerTypeFilter));
        }

        //sort
        Expression<Func<CustomerReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "username" => x => x.UserReadModel.UserName,
            "userid" => x => x.UserId,
            _ => x => x.UserId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            customerQuery = customerQuery.OrderByDescending(keySelector);
        }
        else
        {
            customerQuery = customerQuery.OrderBy(keySelector);
        }

        //paged
        var categories = customerQuery
            .Select(a => new CustomerProfileResponse(
                a.UserId.ToGuid(),
                a.UserReadModel.UserName,
                a.UserReadModel.DateOfBirth,
                a.UserReadModel.ImageUrl!,
                a.UserReadModel.Gender.ToString(),
                a.UserReadModel.PhoneNumber,
                a.UserReadModel.Email,
                a.CustomerType.ToString(),
                a.UserReadModel.UserStatus.ToString())).AsQueryable();

        var categoriesList = await PagedList<CustomerProfileResponse>
            .CreateAsync(categories, page ?? 1, pageSize ?? 10);

        return categoriesList;
    }

    public async Task<bool> IsAccountVerified(
        Email email, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers.AnyAsync(
                a => a.UserReadModel.Email == email.Value 
                && a.UserReadModel.IsVerify == true, 
                cancellationToken);
    }
}
