using Domain.Entities.ProductVariants;
using MediatR;

namespace Domain.Events.ProductVariantEvents;
public record ProductVariantUpdatedEvent(ProductVariantId ProductVariantId) : INotification;

