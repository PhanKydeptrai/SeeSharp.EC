namespace Domain.Utilities.Events.EmployeeEvents;

/// <summary>
/// Gửi mail để xác nhận đổi mật khẩu cho nhân viên
/// </summary>
public record EmployeeChangePasswordEvent(
    Guid UserId, 
    Guid VerificationTokenId, 
    string Email,
    Guid MessageId); 