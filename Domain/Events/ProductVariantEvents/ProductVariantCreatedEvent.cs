using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using MediatR;

namespace Domain.Events.ProductVariantEvents;

public record ProductVariantCreatedEvent(
    ProductVariantId ProductVariantId, 
    ProductId ProductId, 
    CategoryId CategoryId) : INotification;

