namespace Persistence.Outbox.Services;

public interface IOutBoxMessageServices
{
    Task AddNewOutBoxMessageAsync(OutboxMessage outBoxMessage);
    Task UpdateOutStatusBoxMessageAsync(Ulid id, OutboxMessageStatus outboxMessageStatus);
    Task<List<OutboxMessage>> GetPendingOutBoxMessagesAsync(CancellationToken cancellationToken = default);
    Task<List<OutboxMessage>> GetFailedOutBoxMessagesAsync(CancellationToken cancellationToken = default);
    Task<List<OutboxMessage>> GetProcessedOutBoxMessagesAsync(CancellationToken cancellationToken = default);
    Task<OutboxMessage?> GetOutBoxMessageByIdAsync(Ulid id, CancellationToken cancellationToken = default);
    Task DeleteOutBoxMessageAsync(Ulid id);
}
