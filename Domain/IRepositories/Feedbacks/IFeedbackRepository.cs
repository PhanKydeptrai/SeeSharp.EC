using Domain.Entities.Feedbacks;

namespace Domain.IRepositories.Feedbacks;

public interface IFeedbackRepository
{
    /// <summary>
    /// Tạo feedback cho orders
    /// </summary>
    /// <param name="feedback"></param>
    /// <returns></returns>
    Task CreateNewFeedback(Feedback feedback);

    /// <summary>
    /// Lấy feedback theo Id
    /// </summary>
    /// <param name="feedbackId"></param>
    /// <returns></returns>
    Task<Feedback?> GetFeedBackById(FeedbackId feedbackId);
}
