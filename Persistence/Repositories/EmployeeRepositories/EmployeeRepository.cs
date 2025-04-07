using Domain.Entities.Employees;
using Domain.IRepositories.Employees;
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

    
    
}
