using Domain.Entities.Products;
using MediatR;

namespace Domain.Events.ProductEvents;

public record ProductUpdatedEvent(ProductId ProductId) : INotification;
