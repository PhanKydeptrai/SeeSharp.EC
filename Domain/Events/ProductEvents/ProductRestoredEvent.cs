using Domain.Entities.Products;
using MediatR;

namespace Domain.Events.ProductEvents;

public record ProductRestoredEvent(ProductId ProductId) : INotification;