namespace TechnicalTest.Domain.Events.Base
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTimeOffset OccurredOn { get; }
    }
}
