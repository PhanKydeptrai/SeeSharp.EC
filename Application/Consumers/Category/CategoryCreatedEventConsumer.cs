using Domain.Utilities.Events.CategoryEvents;
using MassTransit;

namespace Application.Consumers.Category;

internal sealed class CategoryCreatedEventConsumer : IConsumer<CategoryCreatedEvent>
{
    public Task Consume(ConsumeContext<CategoryCreatedEvent> context)
    {
        throw new NotImplementedException();
    }
}
