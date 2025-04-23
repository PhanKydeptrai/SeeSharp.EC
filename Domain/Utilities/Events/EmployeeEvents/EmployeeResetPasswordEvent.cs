namespace Domain.Utilities.Events.EmployeeEvents;

public record EmployeeResetPasswordEvent(Guid UserId, string Email, string RandomPassword, Guid MessageId); 