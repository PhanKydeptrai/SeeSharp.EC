using Application.Abstractions.EventBus;
using Domain.IRepositories;
using MassTransit;
using Persistence.Outbox.Services;
using System.Text.Json;

namespace Persistence.Outbox;

//TODO: Thêm Batch size cho việc xử lý Outbox
// Thêm thứ tự cho các message
public sealed class OutboxProcessor
{
    private readonly IEventBus _eventBus;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    public OutboxProcessor(
        IEventBus eventBus,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _eventBus = eventBus;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
    }
    public async Task<int> Execute(CancellationToken cancellation = default)
    {
        var outboxMessages = await _outBoxMessageServices.GetPendingOutBoxMessagesAsync(cancellation);

        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                //Lấy loại message để Deserialize
                var messageType = Domain.AssemblyReference.Assembly.GetType(outboxMessage.Type)!;
                var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;
                //Publish message
                await _eventBus.PublishAsync(deserializedMessage, cancellation);
                //Update status
                await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    outboxMessage.Id,
                    OutboxMessageStatus.Processed,
                    string.Empty,
                    DateTime.UtcNow);

                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    outboxMessage.Id,
                    OutboxMessageStatus.Failed,
                    ex.ToString(),
                    DateTime.UtcNow);

                await _unitOfWork.Commit();
            }
        }

        return outboxMessages.Count;
    }


}
