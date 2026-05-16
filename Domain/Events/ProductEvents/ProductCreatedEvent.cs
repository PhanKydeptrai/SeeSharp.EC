using Domain.Entities.Products;
using MediatR;

namespace Domain.Events.ProductEvents;

public record ProductCreatedEvent(ProductId ProductId) : INotification;
