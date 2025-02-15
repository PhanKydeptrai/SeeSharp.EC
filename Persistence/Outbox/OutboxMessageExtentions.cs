using Domain.IRepositories;
using Persistence.Outbox.Services;
using System.Text.Json;

namespace Persistence.Outbox;
internal static class OutboxMessageExtentions
{
    internal static async Task InsertOutboxMessageAsync<T>(
        T message, IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork) where T: notnull
    {
        var outboxMessage = new OutboxMessage
        { 
            Id = Ulid.NewUlid(),
            Type = message.GetType().FullName!,
            Content = JsonSerializer.Serialize(message),
            OccurredOnUtc = DateTime.UtcNow,
            Status = OutboxMessageStatus.Pending                
        };

        await outBoxMessageServices.AddNewOutBoxMessageAsync(outboxMessage);
        await unitOfWork.Commit();
    }
}

