using Application.DTOs.Feedbacks;
using Application.Features.Pages;
using Domain.Entities.Feedbacks;
using Domain.Entities.Products;

namespace Application.IServices;

public interface IFeedbackQueryServices
{
    /// <summary>
    /// Lấy thông tin phản hồi theo ID
    /// </summary>
    /// <returns></returns>
    Task<FeedbackResponse?> GetFeedbackById(FeedbackId feedbackId);

    /// <summary>
    ///  Lấy danh sách phản hồi của sản phẩm
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="filter"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedList<FeedbackResponse>> GetFeedbackOfProduct(
        ProductId productId,
        string? filter,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
}
