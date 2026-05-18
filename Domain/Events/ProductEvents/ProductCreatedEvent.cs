using Domain.Entities.Categories;
using Domain.Entities.Products;
using MediatR;

namespace Domain.Events.ProductEvents;

public record ProductCreatedEvent(ProductId ProductId, CategoryId CategoryId) : INotification;
