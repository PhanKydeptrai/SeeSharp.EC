namespace SharedKernel;

public sealed class OutboxMessage
{
    public Ulid Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public OutboxMessageStatus Status { get; set; }
    public required string Content { get; init; }
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; } = string.Empty;
}

public enum OutboxMessageStatus
{
    Pending,
    Published,
    Processed,
    Failed
}

