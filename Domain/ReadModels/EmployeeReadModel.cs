using Domain.Entities.Employees;

namespace Domain.Database.PostgreSQL.ReadModels;

public class EmployeeReadModel
{
    public Ulid EmployeeId { get; set; }
    public Ulid UserId { get; set; }
    public EmployeeStatus EmployeeStatus { get; set; }
    public Role Role { get; set; }
    public UserReadModel UserReadModel { get; set; } = null!;
}
