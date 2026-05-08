using Domain.Entities.Categories;
using MediatR;

namespace Domain.Events.CategoryEvents;

public record CategoryUpdatedEvent(CategoryId CategoryId) : INotification;