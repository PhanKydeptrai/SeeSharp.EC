using Domain.Entities.Categories;
using MediatR;

namespace Domain.Events.CategoryEvents;

public record CategoryCreatedEvent(CategoryId CategoryId) : INotification;
