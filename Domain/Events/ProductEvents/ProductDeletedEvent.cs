using Domain.Entities.Products;
using MediatR;

namespace Domain.Events.ProductEvents;

public record ProductDeletedEvent(ProductId ProductId) : INotification;