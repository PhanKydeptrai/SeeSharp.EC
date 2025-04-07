using Application.Abstractions.Messaging;
using Application.DTOs.Product;
using Application.Features.Pages;

namespace Application.Features.ProductFeature.Queries.GetAllVariantQuery;

public record GetAllVariantQuery(
    string? filterProductStatus,
    string? filterProduct,
    string? filterCategory,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<ProductVariantResponse>>;
