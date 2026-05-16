using Domain.Entities.ProductVariants;
using MediatR;

namespace Domain.Events.ProductVariantEvents;

public record ProductVariantDeletedEvent(ProductVariantId ProductVariantId) : INotification;

