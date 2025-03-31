using Domain.OutboxMessages.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;
using SharedKernel;

namespace Persistence.Outbox;

internal class OutBoxMessageServices : IOutBoxMessageServices
{
    private readonly SeeSharpPostgreSQLWriteDbContext _dbContext;

    public OutBoxMessageServices(SeeSharpPostgreSQLWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddNewOutBoxMessageAsync(OutboxMessage outBoxMessage)
    {
        await _dbContext.OutboxMessages.AddAsync(outBoxMessage);
    }

    public async Task DeleteOutBoxMessageAsync(Guid id)
    {
        await _dbContext.OutboxMessages
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<List<OutboxMessage>> GetFailedOutBoxMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.OutboxMessages
            .Where(a => a.Status == OutboxMessageStatus.Failed)
            .ToListAsync(cancellationToken);
    }

    public async Task<OutboxMessage?> GetOutBoxMessageByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.OutboxMessages.FindAsync(id, cancellationToken);
    }

    public async Task<List<OutboxMessage>> GetPendingOutBoxMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.OutboxMessages
            .Where(a => a.Status == OutboxMessageStatus.Pending)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<OutboxMessage>> GetProcessedOutBoxMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.OutboxMessages
            .Where(a => a.Status == OutboxMessageStatus.Processed)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateOutStatusBoxMessageAsync(
        Guid id,
        OutboxMessageStatus outboxMessageStatus,
        string? error,
        DateTime processedOnUtc)
    {
        var message = await _dbContext.OutboxMessages.FindAsync(id);
        message!.Status = outboxMessageStatus;
        message.ProcessedOnUtc = processedOnUtc;
        message.Error = error;
    }
}
