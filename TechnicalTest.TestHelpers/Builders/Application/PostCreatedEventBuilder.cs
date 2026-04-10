using TechnicalTest.Domain.Events;

namespace TechnicalTest.TestHelpers.Builders.Application
{
    public class PostCreatedEventBuilder
    {
        private readonly Guid _id;
        private readonly Guid _authorId;
        private readonly string _title;
        private readonly string _description;
        private readonly string _content;
        private readonly DateTimeOffset _occurredAt;

        public static PostCreatedEventBuilder Default() => new();

        private PostCreatedEventBuilder()
        {
            _id = Guid.NewGuid();
            _authorId = Guid.NewGuid();
            _title = "Test title";
            _description = "Test description";
            _content = "Test content";
            _occurredAt = DateTimeOffset.UtcNow;
        }

        public PostCreatedEvent Build() => new(
            _id,
            _authorId,
            _title,
            _description,
            _content,
            _occurredAt
        );
    }
}
