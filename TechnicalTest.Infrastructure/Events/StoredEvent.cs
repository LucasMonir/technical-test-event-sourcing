namespace TechnicalTest.Infrastructure.Events
{
    internal sealed class StoredEvent
    {
        public long Id { get; init; }
        public required string StreamId { get; init; }
        public required int Version { get; init; }
        public required string EventType { get; init; }
        public required string EventData { get; init; }
        public required DateTimeOffset OccurredAt { get; init; }
    }
}
