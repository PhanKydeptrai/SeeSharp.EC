using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;

namespace Persistence.Outbox.Services;

internal class OutBoxMessageServices : IOutBoxMessageServices
{
    private readonly NextSharpMySQLDbContext _dbContext;

    public OutBoxMessageServices(NextSharpMySQLDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddNewOutBoxMessageAsync(OutboxMessage outBoxMessage)
    {
        await _dbContext.OutboxMessages.AddAsync(outBoxMessage);
    }

    public async Task DeleteOutBoxMessageAsync(Ulid id)
    {
        await _dbContext.OutboxMessages
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<List<OutboxMessage>> GetFailedOutBoxMessagesAsync()
    {
        return await _dbContext.OutboxMessages
            .Where(a => a.Status == OutboxMessageStatus.Failed)
            .ToListAsync();
    }

    public async Task<OutboxMessage?> GetOutBoxMessageByIdAsync(Ulid id)
    {
        return await _dbContext.OutboxMessages.FindAsync(id);
    }

    public async Task<List<OutboxMessage>> GetPendingOutBoxMessagesAsync()
    {
        return await _dbContext.OutboxMessages
            .Where(a => a.Status == OutboxMessageStatus.Pending)
            .ToListAsync();
    }

    public async Task<List<OutboxMessage>> GetProcessedOutBoxMessagesAsync()
    {
        return await _dbContext.OutboxMessages
            .Where(a => a.Status == OutboxMessageStatus.Processed)
            .ToListAsync();
    }

    public async Task UpdateOutBoxMessageAsync(Ulid id, OutboxMessageStatus outboxMessageStatus)
    {
        await _dbContext.OutboxMessages
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(
                a => a.SetProperty(
                    p => p.Status, 
                    outboxMessageStatus));
    }
}
