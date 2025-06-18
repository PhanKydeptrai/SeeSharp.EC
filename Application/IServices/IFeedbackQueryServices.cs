using Application.DTOs.Feedbacks;
using Domain.Entities.Feedbacks;

namespace Application.IServices;

public interface IFeedbackQueryServices
{
    /// <summary>
    /// Lấy thông tin phản hồi theo ID
    /// </summary>
    /// <returns></returns>
    Task<FeedbackResponse?> GetFeedbackById(FeedbackId feedbackId);
}
