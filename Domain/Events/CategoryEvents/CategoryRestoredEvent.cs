using Domain.Entities.Categories;
using MediatR;

namespace Domain.Events.CategoryEvents;

public record CategoryRestoredEvent(CategoryId CategoryId) : INotification;