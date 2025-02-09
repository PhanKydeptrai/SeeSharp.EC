namespace Persistence.Database.PostgreSQL.ReadModels;

public class EmployeeReadModel
{
    public Ulid EmployeeId { get; set; }
    public Ulid UserId { get; set; }
    public string EmployeeStatus { get; set; } = null!;
    public string Role { get; set; } = null!;
    public UserReadModel UserReadModel { get; set; } = null!;
}
