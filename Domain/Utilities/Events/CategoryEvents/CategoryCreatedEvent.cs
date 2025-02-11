using Domain.Entities.Categories;

namespace Domain.Utilities.Events.CategoryEvents;
public sealed record CategoryCreatedEvent(Category category);
