namespace TechnicalTest.Application.Abstractions.Events
{
    public interface IEventStore
    {
        Task AppendAsync(
          string streamId,
          int expectedVersion,
          IReadOnlyCollection<object> events,
          CancellationToken cancellationToken = default
        );
    }
}
