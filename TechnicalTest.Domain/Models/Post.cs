using TechnicalTest.Domain.Events;
using TechnicalTest.Domain.Events.Base;

namespace TechnicalTest.Domain.Models
{
    public class Post
    {

        public Guid Id { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Title { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string Content { get; private set; } = default!;

        private readonly List<IDomainEvent> _events = [];
        public IReadOnlyCollection<IDomainEvent> Events => _events;

        private Post() { }

        public static Post Create(
            Guid postId,
            Guid authorId,
            string title,
            string description,
            string content)
        {
            var post = new Post();

            var @event = new PostCreatedEvent(
                postId,
                authorId,
                title,
                description,
                content,
                DateTimeOffset.UtcNow
            );

            post.Apply(@event);
            post._events.Add(@event);

            return post;
        }

        public static Post FromHistory(IEnumerable<IDomainEvent> events)
        {
            var post = new Post();

            foreach (var e in events)
            {
                post.Apply((dynamic)e);
            }

            return post;
        }

        public void Apply(PostCreatedEvent e)
        {
            Id = e.PostId;
            AuthorId = e.AuthorId;
            Title = e.Title;
            Description = e.Description;
            Content = e.Content;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
