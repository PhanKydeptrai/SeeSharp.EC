using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.Features.Pages;

namespace Application.Features.CategoryFeature.Queries.GetAllCategory;

public sealed record GetAllCategoryQuery(
    string? filter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<CategoryResponse>>;

