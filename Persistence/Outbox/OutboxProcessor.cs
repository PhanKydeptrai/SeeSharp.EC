using Application.Abstractions.EventBus;
using Domain;
using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using MassTransit;
using SharedKernel;
using System.Text.Json;

namespace Persistence.Outbox;

//TODO: Thêm Batch size cho việc xử lý Outbox
// Thêm thứ tự cho các message
public sealed class OutboxProcessor
{
    private readonly IEventBus _eventBus;
    private readonly IPublishEndpoint _publishEndpoint;
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
        _publishEndpoint = publishEndpoint;
    }
    public async Task<int> Execute(CancellationToken cancellation = default)
    {
        var outboxMessages = await _outBoxMessageServices.GetPendingOutBoxMessagesAsync(cancellation);

        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                //Lấy loại message để Deserialize
                var messageType = AssemblyReference.Assembly.GetType(outboxMessage.Type)!;
                var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;
                //Publish message
                //await _eventBus.PublishAsync(deserializedMessage, cancellation);
                await _publishEndpoint.Publish(deserializedMessage, messageType, cancellation);
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
                    OutboxMessageStatus.Pending,//NOTE: Change to Failed
                    ex.ToString(),
                    DateTime.UtcNow);

                await _unitOfWork.Commit();
            }
        }

        return outboxMessages.Count;
    }


}
