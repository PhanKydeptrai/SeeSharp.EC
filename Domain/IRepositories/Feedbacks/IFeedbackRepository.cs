using Domain.Entities.Customers;
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

    /// <summary>
    /// Lấy feedback của khách hàng theo Id
    /// Nếu không tìm thấy trả về null
    /// </summary>
    /// <param name="feedbackId"></param>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<Feedback?> GetFeedBackOfCustomerById(FeedbackId feedbackId, CustomerId customerId);
}
