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
}
