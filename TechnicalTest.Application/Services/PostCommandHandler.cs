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
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IAuthorResolver _authorResolver = authorResolver;
        private readonly IEventStore _eventStore = eventStore;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
            await _postRepository.CreatePostAsync(post);

            await _unitOfWork.CommitAsync();

            return post.Id;
        }

        //private async Task<Guid> GetAuthor(CreatePostCommand command)
        //{
        //    if (command.AuthorId.HasValue)
        //        return await ResolveExistingAuthorId(command.AuthorId.Value);

        //    if (command.Author is not null)
        //        return await CreateNewAuthorId(command.Author);

        //    throw new InvalidOperationException("Cannot create post: no author provided.");
        //}

        //private async Task<Guid> CreateNewAuthorId(AuthorModel authorModel)
        //{
        //    var name = authorModel.Name?.Trim();
        //    var surname = authorModel.Surname?.Trim();

        //    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
        //        throw new InvalidOperationException("Author name and surname are required.");

        //    return await _authorRepository.CreateAuthorAsync(Author.Create(name, surname));
        //}

        //private async Task<Guid> ResolveExistingAuthorId(Guid authorId)
        //{
        //    var author = await _authorRepository.GetPostAuthorAsync(authorId);

        //    if (author is null)
        //        throw new InvalidOperationException("AuthorId not found.");

        //    return author.Id;
        //}
    }
}
