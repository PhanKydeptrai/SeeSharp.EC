namespace Domain.Utilities.Events.CustomerEvents;

/// <summary>
/// Gửi mail để xác nhận đổi mật khẩu cho khách hàng
/// </summary>
/// <param name="UserId"></param>
/// <param name="VerificationTokenId"></param>
/// <param name="Email"></param>
/// <param name="MessageId"></param>
public record CustomerChangePasswordIntergrationEvent(
    Guid UserId, 
    Guid VerificationTokenId, 
    string Email,
    Guid MessageId);