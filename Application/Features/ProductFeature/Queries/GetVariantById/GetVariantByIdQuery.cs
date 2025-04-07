using Application.Abstractions.Messaging;
using Application.DTOs.Product;

namespace Application.Features.ProductFeature.Queries.GetVariantById;

public record GetVariantByIdQuery(Guid ProductVariantId) : IQuery<ProductVariantResponse>;
