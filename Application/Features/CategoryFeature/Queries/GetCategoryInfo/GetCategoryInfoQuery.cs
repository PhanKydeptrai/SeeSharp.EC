using Application.Abstractions.Messaging;
using Application.DTOs.Category;

namespace Application.Features.CategoryFeature.Queries.GetCategoryInfo;

public record GetCategoryInfoQuery() : IQuery<List<CategoryInfo>>;


