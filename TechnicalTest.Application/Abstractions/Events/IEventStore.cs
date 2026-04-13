using TechnicalTest.Domain.Events.Base;

namespace TechnicalTest.Application.Abstractions.Events
{
    public interface IEventStore
    {
        Task AppendAsync(
            string streamId,
            int expectedVersion,
            IReadOnlyCollection<IDomainEvent> events,
            CancellationToken cancellationToken = default);

        Task<(IReadOnlyCollection<IDomainEvent> Events, int Version)> LoadAsync(
            string streamId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<StoredEventEnvelope>> LoadAllAsync(
            long afterId,
            int take = 100,
            CancellationToken cancellationToken = default);
    }

    public record StoredEventEnvelope(
        long Id,
        string StreamId,
        int Version,
        IDomainEvent Event,
        DateTimeOffset OccurredAt
    );
}