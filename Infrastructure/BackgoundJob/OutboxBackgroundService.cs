using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Outbox;
using Quartz;

namespace Infrastructure.BackgoundJob;

public class OutboxBackgroundService : IJob
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<OutboxBackgroundService> _logger;
    public OutboxBackgroundService(
        ILogger<OutboxBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Starting OutboxBackgroundService...");

            using var scope = serviceScopeFactory.CreateScope();

            var outboxProcessor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();

            await outboxProcessor.Execute();
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("OutboxBackgroundService cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in OutboxBackgroundService");
        }
        finally
        {
            _logger.LogInformation("OutboxBackgroundService finished...");
        }

    }
}




