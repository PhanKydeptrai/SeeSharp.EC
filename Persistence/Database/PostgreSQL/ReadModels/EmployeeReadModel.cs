namespace Persistence.Database.PostgreSQL.ReadModels;

public class EmployeeReadModel
{
    public string EmployeeId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string EmployeeStatus { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual UserReadModel User { get; set; } = null!;
}
