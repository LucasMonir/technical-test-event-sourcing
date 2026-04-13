using TechnicalTest.Domain.Events;
using TechnicalTest.Domain.Events.Base;

namespace TechnicalTest.Domain.Models
{
    public class Author
    {
        private readonly List<IDomainEvent> _events = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Surname { get; private set; } = default!;

        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        private Author() { }

        public static Author Create(Guid authorId, string name, string surname)
        {
            var author = new Author();

            var @event = new AuthorCreatedEvent(
                authorId,
                name,
                surname,
                DateTimeOffset.UtcNow
            );

            author._events.Add(@event);
            author.Apply((dynamic)@event);

            return author;
        }

        public void Apply(AuthorCreatedEvent e)
        {
            Id = e.AuthorId;
            Name = e.Name;
            Surname = e.Surname;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
