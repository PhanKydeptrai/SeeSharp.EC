using Application.DTOs.Category;
using Application.Features.Pages;
using Domain.Entities.Categories;

namespace Application.IServices;

public interface ICategoryQueryServices
{
    /// <summary>
    /// Lấy danh sách danh mục theo các điều kiện lọc, tìm kiếm, phân trang
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
    // Lấy thông tin chi tiết của một danh mục theo id lấy cả những danh mục đã bị xóa
    /// <summary>
    /// Lấy thông tin chi tiết của một danh mục theo id lấy cả những danh mục đã bị xóa
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CategoryResponse?> GetById(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default);

    // Lấy thông tin chi tiết của một danh mục theo id trừ những danh mục đã bị xóa
    Task<CategoryResponse?> GetCategoryDetail(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default);

    Task<bool> IsCategoryNameExist(
        CategoryId? categoryId,
        CategoryName categoryName,
        CancellationToken cancellationToken = default);
    
    Task<bool> IsCategoryStatusNotDeleted(
        CategoryId categoryId,
        CancellationToken cancellationToken = default);
    
    Task<List<CategoryInfo>> GetCategoryInfo();
}
