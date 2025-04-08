using Domain.Entities.Employees;
using Domain.Entities.Users;
using Domain.IRepositories.Employees;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.EmployeeRepositories;

internal sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLDbContext;
    public EmployeeRepository(SeeSharpPostgreSQLWriteDbContext postgreSQLDbContext)
    {
        _postgreSQLDbContext = postgreSQLDbContext;
    }

    public async Task AddEmployee(Employee employee)
    {
        await _postgreSQLDbContext.Employees.AddAsync(employee);
    }

    public async Task<Employee?> GetEmployeeByUserId(UserId userId)
    {
        return await _postgreSQLDbContext.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }
}
