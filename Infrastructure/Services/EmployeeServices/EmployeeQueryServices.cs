using Application.DTOs.Customer;
using Application.DTOs.Employee;
using Application.IServices;
using Domain.Entities.Employees;
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

    public async Task<EmployeeAuthenticationResponse?> AuthenticateEmployee(
        Email email, PasswordHash password)
    {
        return await _dbContext.Employees.Where(
            a => a.UserReadModel.Email == email.Value 
            && a.UserReadModel.PasswordHash == password.Value
            && a.UserReadModel.IsVerify == true
            && a.UserReadModel.UserStatus != (int)UserStatus.Deleted
            && a.UserReadModel.UserStatus != (int)UserStatus.Blocked)
            .Select(a => new EmployeeAuthenticationResponse(
                a.UserReadModel.UserId,
                a.EmployeeId,
                a.UserReadModel.Email,
                a.UserReadModel.UserStatus.ToString(),
                a.Role.ToString()))
            .FirstOrDefaultAsync();
    }
    
    public async Task<Employee?> GetEmployeeByEmail(Email email)
    {
        var employeeData = await _dbContext.Employees
            .Where(e => e.UserReadModel.Email == email.Value)
            .Select(e => new 
            {
                EmployeeId = e.EmployeeId,
                UserId = e.UserReadModel.UserId,
                Role = e.Role
            })
            .FirstOrDefaultAsync();
            
        if (employeeData == null)
            return null;
            
        return Employee.FromExisting(
            EmployeeId.FromGuid(employeeData.EmployeeId.ToGuid()),
            UserId.FromGuid(employeeData.UserId.ToGuid()),
            (Role)employeeData.Role);
    }
    
    public async Task<Employee?> GetEmployeeById(UserId userId)
    {
        var employeeData = await _dbContext.Employees
            .Include(e => e.UserReadModel)
            .Where(e => e.UserId == new Ulid(userId))
            .Select(e => new 
            {
                EmployeeId = e.EmployeeId,
                UserId = e.UserReadModel.UserId,
                Role = e.Role
            })
            .FirstOrDefaultAsync();
            
        if (employeeData == null)
            return null;
            
        return Employee.FromExisting(
            EmployeeId.FromGuid(employeeData.EmployeeId.ToGuid()),
            UserId.FromGuid(employeeData.UserId.ToGuid()),
            (Role)employeeData.Role);
    }

    public async Task<EmployeeProfileResponse?> GetEmployeeProfileById(UserId userId)
    {
        return await _dbContext.Employees
            .Where(a => a.UserId == new Ulid(userId.Value))
            .Select(a => new EmployeeProfileResponse(
                a.UserId.ToGuid(),
                a.UserReadModel.UserName,
                a.UserReadModel.DateOfBirth,
                a.UserReadModel.ImageUrl, // TODO: Xử lý imageUrl sẽ được triển khai sau
                a.UserReadModel.PhoneNumber,
                a.UserReadModel.Email,
                a.Role.ToString(),
                a.UserReadModel.UserStatus.ToString()))
            .FirstOrDefaultAsync();
    }
}
