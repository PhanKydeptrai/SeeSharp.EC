using Application.IServices;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.EmployeeServices;

public class EmployeeQueryServices : IEmployeeQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;
    public EmployeeQueryServices(SeeSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsEmailExists(Email email)
    {
        return await _dbContext.Employees.AnyAsync(a => a.UserReadModel.Email == email.Value);
    }

    public async Task<bool> IsPhoneNumberExists(PhoneNumber phoneNumber)
    {
        return await _dbContext.Employees.AnyAsync(a => a.UserReadModel.PhoneNumber == phoneNumber.Value);   
    }

    

}
