namespace Domain.Database.PostgreSQL.ReadModels;

public class EmployeeReadModel
{
    public Guid EmployeeId { get; set; }
    public Guid UserId { get; set; }
    public string EmployeeStatus { get; set; } = null!;
    public string Role { get; set; } = null!;
    public UserReadModel UserReadModel { get; set; } = null!;
}
