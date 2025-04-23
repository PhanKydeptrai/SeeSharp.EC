using Domain.Entities.Users;

namespace Domain.Entities.Employees;

public sealed class Employee
{
    public EmployeeId EmployeeId { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public Role Role { get; private set; }
    public User? User { get; set; } = null!;

    public Employee(
        EmployeeId employeeId, 
        UserId userId, 
        Role role)
    {
        EmployeeId = employeeId;
        UserId = userId;
        Role = role;
    }

    //factory method
    public static Employee NewEmployee(
        UserId userId)
    {
        return new Employee(EmployeeId.New(), userId, Role.Employee);
    }

    public static Employee NewAdmin( 
        UserId userId)
    {
        return new Employee(EmployeeId.New(), userId, Role.Admin);
    }

    public static Employee FromExisting(
        EmployeeId employeeId,
        UserId userId,
        Role role)
    {
        return new Employee(employeeId, userId, role);
    }

    
}

