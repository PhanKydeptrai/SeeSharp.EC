using Domain.OutboxMessages.Services;
using SharedKernel;
using System.Text.Json;

namespace Application.Outbox;
public static class OutboxMessageExtentions
{
    public static async Task InsertOutboxMessageAsync<T>(
        Ulid messageId,
        T message, IOutBoxMessageServices outBoxMessageServices) where T: notnull
    {
        var outboxMessage = new OutboxMessage
        { 
            Id = messageId,
            Type = message.GetType().FullName!,
            Content = JsonSerializer.Serialize(message),
            Error = string.Empty,
            OccurredOnUtc = DateTime.UtcNow,
            Status = OutboxMessageStatus.Pending
        };

        await outBoxMessageServices.AddNewOutBoxMessageAsync(outboxMessage);
    }
}

