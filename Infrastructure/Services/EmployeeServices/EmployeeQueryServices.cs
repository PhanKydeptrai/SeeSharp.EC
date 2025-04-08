using Application.DTOs.Employee;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Employees;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;
using System.Linq.Expressions;

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
            && a.UserReadModel.UserStatus != UserStatus.Deleted
            && a.UserReadModel.UserStatus != UserStatus.Blocked)
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
    
    public async Task<PagedList<EmployeeResponse>> GetAllEmployees(
        string? statusFilter,
        string? roleFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var query = _dbContext.Employees.AsQueryable();
        
        // Filter by status
        if (!string.IsNullOrEmpty(statusFilter))
        {
            query = query.Where(e => e.UserReadModel.UserStatus == (UserStatus)Enum.Parse(typeof(UserStatus), statusFilter));
        }
        else
        {
            // By default, exclude deleted employees
            query = query.Where(e => e.UserReadModel.UserStatus != UserStatus.Deleted);
        }
        
        // Filter by role
        if (!string.IsNullOrEmpty(roleFilter))
        {
            query = query.Where(e => e.Role == (Role)Enum.Parse(typeof(Role), roleFilter, true));
        }
        
        // Search by name, email, or phone
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(e => 
                e.UserReadModel.UserName.ToLower().Contains(searchTerm) ||
                e.UserReadModel.Email.ToLower().Contains(searchTerm) ||
                e.UserReadModel.PhoneNumber.Contains(searchTerm));
        }
        
        // Sort
        Expression<Func<EmployeeReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "username" => e => e.UserReadModel.UserName,
            _ => e => e.EmployeeId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(keySelector);
        }
        else
        {
            query = query.OrderBy(keySelector);
        }
        
        // Map to EmployeeResponse
        var employeeResponses = query.Select(e => new EmployeeResponse(
            e.UserId.ToGuid(),
            e.UserReadModel.UserName,
            e.UserReadModel.DateOfBirth,
            e.UserReadModel.ImageUrl,
            e.UserReadModel.PhoneNumber,
            e.UserReadModel.Email,
            e.Role.ToString(),
            e.UserReadModel.UserStatus.ToString(),
            DateTime.Now)).AsQueryable(); // Using current date as we don't have CreatedAt
        
        // Apply pagination
        return await PagedList<EmployeeResponse>.CreateAsync(employeeResponses, page ?? 1, pageSize ?? 10);
    }
}
