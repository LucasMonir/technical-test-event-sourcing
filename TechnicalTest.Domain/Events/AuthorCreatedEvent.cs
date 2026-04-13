using TechnicalTest.Domain.Events.Base;

namespace TechnicalTest.Domain.Events
{
    public record AuthorCreatedEvent(
        Guid AuthorId,
        string Name,
        string Surname,
        DateTimeOffset OccurredOn
    ) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}
