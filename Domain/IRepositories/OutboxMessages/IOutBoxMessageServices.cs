using SharedKernel;

namespace Domain.OutboxMessages.Services;

public interface IOutBoxMessageServices
{
    Task AddNewOutBoxMessageAsync(OutboxMessage outBoxMessage);
    Task UpdateOutStatusBoxMessageAsync(Guid id, OutboxMessageStatus outboxMessageStatus, string? error, DateTime processedOnUtc);
    Task<List<OutboxMessage>> GetPendingOutBoxMessagesAsync(CancellationToken cancellationToken = default);
    Task<List<OutboxMessage>> GetFailedOutBoxMessagesAsync(CancellationToken cancellationToken = default);
    Task<List<OutboxMessage>> GetProcessedOutBoxMessagesAsync(CancellationToken cancellationToken = default);
    Task<OutboxMessage?> GetOutBoxMessageByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteOutBoxMessageAsync(Guid id);
}
