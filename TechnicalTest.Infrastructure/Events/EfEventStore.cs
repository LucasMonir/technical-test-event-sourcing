using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Domain.Events;
using TechnicalTest.Domain.Events.Base;

namespace TechnicalTest.Infrastructure.Events
{
    internal sealed class EfEventStore(AppDbContext db) : IEventStore
    {
        private readonly AppDbContext _db = db;

        public async Task AppendAsync(
            string streamId,
            int expectedVersion,
            IReadOnlyCollection<IDomainEvent> events,
            CancellationToken cancellationToken = default)
        {
            EnsureValidStreamId(streamId);

            var currentVersion =
                await _db.StoredEvents
                    .Where(e => e.StreamId == streamId)
                    .OrderByDescending(e => e.Version)
                    .Select(e => (int?)e.Version)
                    .FirstOrDefaultAsync(cancellationToken)
                ?? 0;

            if (currentVersion != expectedVersion)
                throw new InvalidOperationException(
                    $"Concurrency conflict on {streamId}. Expected {expectedVersion}, got {currentVersion}.");

            var nextVersion = currentVersion;

            foreach (var @event in events)
            {
                nextVersion++;

                _db.StoredEvents.Add(new StoredEvent
                {
                    StreamId = streamId,
                    Version = nextVersion,
                    EventType = @event.GetType().Name,
                    EventData = JsonSerializer.Serialize(@event, @event.GetType()),
                    OccurredAt = @event.OccurredOn
                });
            }

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyCollection<IDomainEvent> Events, int Version)> LoadAsync(
            string streamId,
            CancellationToken cancellationToken = default)
        {
            EnsureValidStreamId(streamId);

            var stored = await _db.StoredEvents
                .Where(e => e.StreamId == streamId)
                .OrderBy(e => e.Version)
                .ToListAsync(cancellationToken);

            var events = stored
                .Select(e => Deserialize(e.EventType, e.EventData))
                .ToList();

            var version = stored.Count == 0 ? 0 : stored.Max(e => e.Version);

            return (events, version);
        }

        public async Task<IReadOnlyCollection<StoredEventEnvelope>> LoadAllAsync(
            long afterId,
            int take = 100,
            CancellationToken cancellationToken = default)
        {
            var stored = await _db.StoredEvents
                .Where(e => e.Id > afterId)
                .OrderBy(e => e.Id)
                .Take(take)
                .ToListAsync(cancellationToken);

            return [.. stored
                .Select(e => new StoredEventEnvelope(
                    e.Id,
                    e.StreamId,
                    e.Version,
                    Deserialize(e.EventType, e.EventData),
                    e.OccurredAt
                ))];
        }

        private static IDomainEvent Deserialize(string type, string data)
        {
            return type switch
            {
                nameof(PostCreatedEvent) =>
                    JsonSerializer.Deserialize<PostCreatedEvent>(data)!,

                nameof(AuthorCreatedEvent) =>
                    JsonSerializer.Deserialize<AuthorCreatedEvent>(data)!,

                _ => throw new Exception($"Unknown event type: {type}")
            };
        }

        private static void EnsureValidStreamId(string streamId)
        {
            if (string.IsNullOrWhiteSpace(streamId))
                throw new ArgumentException("StreamId is required.", nameof(streamId));
        }
    }
}
