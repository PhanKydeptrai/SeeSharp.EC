using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using MediatR;

namespace Domain.Events.ProductEvents;

public record ProductUpdatedEvent(
    ProductId ProductId, 
    ProductVariantId VariantId,
    CategoryId CategoryId) : INotification;
