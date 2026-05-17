using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using MediatR;

namespace Domain.Events.ProductVariantEvents;

public record ProductVariantDeletedEvent(ProductVariantId ProductVariantId, ProductId ProductId) : INotification;

