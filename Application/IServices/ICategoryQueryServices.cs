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
        int? page = 1,
        int? pageSize = 10);

    Task<CategoryResponse?> GetById(CategoryId categoryId, CancellationToken cancellationToken = default);
}
