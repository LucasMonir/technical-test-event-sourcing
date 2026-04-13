using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Domain.Models;

namespace TechnicalTest.Application.Services
{
    internal class AuthorResolver(IEventStore eventStore) : IAuthorResolver
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<Guid> ResolveAsync(CreatePostCommand command, CancellationToken cancellationToken = default)
        {
            if (command.AuthorId.HasValue)
            {
                var streamId = $"author-{command.AuthorId.Value}";
                var (events, _) = await _eventStore.LoadAsync(streamId, cancellationToken);

                if (events.Count == 0)
                    throw new InvalidOperationException($"Author {command.AuthorId.Value} does not exist.");

                return command.AuthorId.Value;
            }

            if (command.Author is null)
                throw new InvalidOperationException("AuthorId or Author object must be provided.");

            var authorId = Guid.NewGuid();
            var streamIdNew = $"author-{authorId}";

            var author = Author.Create(
                          authorId,
                          command.Author.Name,
                          command.Author.Surname
            );

            await _eventStore.AppendAsync(
                streamIdNew,
                expectedVersion: 0,
                author.Events,
                cancellationToken
            );

            author.ClearEvents();

            return authorId;
        }
    }
}
