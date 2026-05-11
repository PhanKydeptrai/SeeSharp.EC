using Domain.Entities.Categories;
using MediatR;

namespace Domain.Events.CategoryEvents;

public record CategoryDeletedEvent(CategoryId CategoryId) : INotification;
