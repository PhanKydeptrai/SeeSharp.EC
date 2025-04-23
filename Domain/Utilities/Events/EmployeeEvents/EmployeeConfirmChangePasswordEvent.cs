namespace Domain.Utilities.Events.EmployeeEvents;

public record EmployeeConfirmChangePasswordEvent(string Email, Guid MessageId); 