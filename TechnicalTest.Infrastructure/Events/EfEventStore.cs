using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TechnicalTest.Application.Abstractions.Events;

namespace TechnicalTest.Infrastructure.Persistence.Events
{
    internal sealed class EfEventStore(AppDbContext db) : IEventStore
    {
        public async Task AppendAsync(string streamId, int expectedVersion, IReadOnlyCollection<object> events, CancellationToken ct = default)
        {
            var currentVersion =
                await db.StoredEvents
                    .Where(e => e.StreamId == streamId)
                    .OrderByDescending(e => e.Version)
                    .Select(e => (int?)e.Version)
                    .FirstOrDefaultAsync(ct)
                ?? 0;

            if (currentVersion != expectedVersion)
                throw new InvalidOperationException($"Concurrency conflict on {streamId}. Expected {expectedVersion}, got {currentVersion}.");

            var nextVersion = expectedVersion;

            foreach (var @event in events)
            {
                nextVersion++;

                db.StoredEvents.Add(new StoredEvent
                {
                    StreamId = streamId,
                    Version = nextVersion,
                    EventType = @event.GetType().FullName!,
                    EventData = JsonSerializer.Serialize(@event, @event.GetType()),
                    OccurredAt = DateTimeOffset.UtcNow
                });
            }
        }
    }
}
