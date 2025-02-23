using Application.Abstractions.Messaging;
using Application.DTOs.Product;

namespace Application.Features.ProductFeature.Queries.GetProductById;

// public record GetProductByIdQuery(Ulid productId) : IQuery<ProductResponse>;

public record GetProductByIdQuery(Guid productId) : IQuery<ProductResponse>;

