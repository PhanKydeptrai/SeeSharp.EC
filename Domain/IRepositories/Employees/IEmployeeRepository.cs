using Domain.Entities.Employees;
using Domain.Entities.Users;

namespace Domain.IRepositories.Employees;

public interface IEmployeeRepository
{
    Task AddEmployee(Employee employee);
    Task<Employee?> GetEmployeeByUserId(UserId userId);
    Task<Employee?> IsEmployeeExist(Email email);
    // Task<Employee?> GetEmployee(EmployeeId employeeId);
    // Task<Employee?> GetEmployeeByEmail(string email);
    // Task<Employee?> GetEmployeeByPhoneNumber(string phoneNumber);
}
