using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using SharedKernel;
using System.Text.Json;

namespace Persistence.Outbox;
public static class OutboxMessageExtentions
{
    public static async Task InsertOutboxMessageAsync<T>(
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

