namespace Domain.Utilities.Events.EmployeeEvents;

public record EmployeeResetPasswordEmailSendEvent(
    Guid UserId, 
    Guid VerificationTokenId, 
    string Email, 
    Guid MessageId); 