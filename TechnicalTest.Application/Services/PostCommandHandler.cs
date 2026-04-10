using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Persistence;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Domain;
using TechnicalTest.Domain.Events;

namespace TechnicalTest.Application.Services
{
    internal class PostCommandHandler(IPostRepository postRepository,
        IAuthorResolver authorResolver,
        IEventStore eventStore,
        IUnitOfWork unitOfWork)
        : IPostCommandHandler
    {
        private readonly IAuthorResolver _authorResolver = authorResolver;
        private readonly IEventStore _eventStore = eventStore;

        public async Task<Guid> Handle(CreatePostCommand command)
        {
            Guid authorId = await _authorResolver.ResolveAsync(command);

            var post = Post.Create(
                authorId,
                command.Title,
                command.Description,
                command.Content
            );

            var postCreated = new PostCreatedEvent(
                post.Id,
                post.AuthorId,
                post.Title,
                post.Description,
                post.Content,
                DateTimeOffset.UtcNow);

            await _eventStore.AppendAsync($"post-{post.Id}", expectedVersion: 0, [postCreated]);

            return post.Id;
        }
    }
}
