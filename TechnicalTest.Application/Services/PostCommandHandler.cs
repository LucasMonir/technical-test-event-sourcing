using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Domain.Models;

namespace TechnicalTest.Application.Services
{
    internal class PostCommandHandler(
        IAuthorResolver authorResolver,
        IEventStore eventStore)
        : IPostCommandHandler
    {
        private readonly IAuthorResolver _authorResolver = authorResolver;
        private readonly IEventStore _eventStore = eventStore;

        public async Task<Guid> Handle(CreatePostCommand command)
        {
            Guid authorId = await _authorResolver.ResolveAsync(command);
            Guid postId = Guid.NewGuid();

            var streamId = $"post-{postId}";

            var (history, version) = await _eventStore.LoadAsync(streamId);

            var post = history is not null && history.Count != 0
                ? Post.FromHistory(history)
                : Post.Create(
                    postId,
                    authorId,
                    command.Title,
                    command.Description,
                    command.Content
                );

            await _eventStore.AppendAsync(
                streamId,
                version,
                post.Events
            );

            post.ClearEvents();

            return post.Id;
        }
    }
}
