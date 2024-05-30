namespace Infraestructure.Outbox;

internal sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessOnUtc { get; set; }
    public string? Error { get; set; }

}
