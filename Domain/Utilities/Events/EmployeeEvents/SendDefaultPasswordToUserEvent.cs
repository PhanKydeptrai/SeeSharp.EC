namespace Domain.Utilities.Events.EmployeeEvents;

public record SendDefaultPasswordToUserEvent(string email, string randomPassword, Guid MessageId);