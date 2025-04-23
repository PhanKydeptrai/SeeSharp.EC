namespace Domain.Utilities.Events.EmployeeEvents;

public record SendDefaultPasswordToUserEvent(string Email, string RandomPassword, Guid MessageId);