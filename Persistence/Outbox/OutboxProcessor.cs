using Domain;
using Domain.OutboxMessages.Services;
using MassTransit;
using SharedKernel;
using System.Text.Json;

namespace Persistence.Outbox;

//TODO: Thêm Batch size cho việc xử lý Outbox
//!FIXME: Sửa phương thức update
// Thêm thứ tự cho các message
public sealed class OutboxProcessor
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public OutboxProcessor(
        IOutBoxMessageServices outBoxMessageServices,
        IPublishEndpoint publishEndpoint)
    {
        _outBoxMessageServices = outBoxMessageServices;
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
                await _publishEndpoint.Publish(deserializedMessage, messageType, cancellation);

                //Update status
                await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    outboxMessage.Id,
                    OutboxMessageStatus.Published,
                    string.Empty,
                    DateTime.UtcNow);

            }
            catch (Exception ex) //cant publish message => update status to failed
            {
                await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    outboxMessage.Id,
                    OutboxMessageStatus.Failed,
                    ex.ToString(),
                    DateTime.UtcNow);
            }
        }

        return outboxMessages.Count;
    }


}
