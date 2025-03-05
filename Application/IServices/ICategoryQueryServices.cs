using Application.DTOs.Category;
using Application.Features.Pages;
using Domain.Entities.Categories;

namespace Application.IServices;

public interface ICategoryQueryServices
{
    Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);

    Task<CategoryResponse?> GetById(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default);

    Task<bool> IsCategoryNameExist(
        CategoryId? categoryId,
        CategoryName categoryName,
        CancellationToken cancellationToken = default);
    
    Task<bool> IsCategoryStatusNotDeleted(
        CategoryId categoryId,
        CancellationToken cancellationToken = default);
}
