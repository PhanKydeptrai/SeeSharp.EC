namespace Persistence.Outbox.Services;

public interface IOutBoxMessageServices
{
    Task AddNewOutBoxMessageAsync(OutboxMessage outBoxMessage);
    Task UpdateOutBoxMessageAsync(Ulid id, OutboxMessageStatus outboxMessageStatus);
    Task<List<OutboxMessage>> GetPendingOutBoxMessagesAsync();
    Task<List<OutboxMessage>> GetFailedOutBoxMessagesAsync();
    Task<List<OutboxMessage>> GetProcessedOutBoxMessagesAsync();
    Task<OutboxMessage?> GetOutBoxMessageByIdAsync(Ulid id);
    Task DeleteOutBoxMessageAsync(Ulid id);
}
