using Application.Abstractions.Messaging;
using Application.DTOs.Category;

namespace Application.Features.CategoryFeature.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid categoryId) : IQuery<CategoryResponse>;
