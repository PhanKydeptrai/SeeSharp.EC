namespace Persistence.Outbox;

public sealed class OutboxMessage
{
    public Ulid Id { get; init; }
    public OutboxMessageStatus Status { get; init; }
    public required string Content { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }
}

public enum OutboxMessageStatus
{
    Pending,
    Processed,
    Failed
}

