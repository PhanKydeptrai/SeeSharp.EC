using Application.Abstractions.Messaging;
using Application.DTOs.Category;

namespace Application.Features.CategoryFeature.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(string categoryId) : IQuery<CategoryResponse>;
