using Domain.Entities.Users;

namespace Domain.Entities.Employees;

public sealed class Employee
{
    public EmployeeId EmployeeId { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public EmployeeStatus EmployeeStatus { get; private set; }
    public Role Role { get; private set; }
    public User? User { get; private set; } = null!;

    public Employee(
        EmployeeId employeeId, 
        UserId userId, 
        Role role, 
        EmployeeStatus employeeStatus)
    {
        EmployeeId = employeeId;
        UserId = userId;
        Role = role;
        EmployeeStatus = employeeStatus;
    }

    //factory method
    public static Employee NewEmployee(
        UserId userId)
    {
        return new Employee(EmployeeId.New(), userId, Role.Employee, EmployeeStatus.Active);
    }

    public static Employee NewAdmin( 
        UserId userId)
    {
        return new Employee(EmployeeId.New(), userId, Role.Admin, EmployeeStatus.Active);
    }

}

