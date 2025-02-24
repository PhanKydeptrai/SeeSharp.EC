using Application.Abstractions.Messaging;
using Application.DTOs.Product;
using Application.Features.Pages;

namespace Application.Features.ProductFeature.Queries.GetAllProduct;

public record GetAllProductQuery(
    string? filterProductStatus,
    string? filterCategory,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<ProductResponse>>;
