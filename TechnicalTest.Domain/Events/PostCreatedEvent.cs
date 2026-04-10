namespace TechnicalTest.Domain.Events
{
    public record PostCreatedEvent(
        Guid PostId,
        Guid AuthorId,
        string Title,
        string Description,
        string Content,
        DateTimeOffset OccurredAt
    );
}
