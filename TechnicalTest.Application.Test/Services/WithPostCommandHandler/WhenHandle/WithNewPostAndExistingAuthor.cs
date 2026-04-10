using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Persistence;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain;
using TechnicalTest.Domain.Events;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Services.WithPostCommandHandler.WhenHandle
{
    public class WithNewPostAndExistingAuthor
    {
        private readonly IPostRepository _postRepository;
        private readonly IAuthorResolver _authorResolver;
        private readonly IEventStore _eventStore;
        private readonly IUnitOfWork _unitOfWork;

        private readonly CreatePostCommand _createPostCommand;

        private readonly Author _author;
        private readonly Post _post;

        private readonly PostCommandHandler _sut;

        public WithNewPostAndExistingAuthor()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _createPostCommand = new CreatePostCommand(
                _post.AuthorId,
                _post.Title,
                _post.Description,
                _post.Content
            );

            _authorResolver = Substitute.For<IAuthorResolver>();
            _authorResolver.ResolveAsync(_createPostCommand)
                .Returns(_author.Id);

            _postRepository = Substitute.For<IPostRepository>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventStore = Substitute.For<IEventStore>();

            _sut = new PostCommandHandler(
                _postRepository,
                _authorResolver,
                _eventStore,
                _unitOfWork);
        }


        [Fact]
        public async Task ThenMustCallRepository()
        {
            var result = await _sut.Handle(_createPostCommand);

            result.Should().NotBeEmpty();

            await _postRepository.Received(1).CreatePostAsync(Arg.Is<Post>(p =>
                p.Id == result &&
                p.AuthorId == _author.Id &&
                p.Title == _post.Title &&
                p.Description == _post.Description &&
                p.Content == _post.Content)
            );
        }

        [Fact]
        public async Task ThenMustCallEventStore()
        {
            var result = await _sut.Handle(_createPostCommand);

            await _eventStore.Received(1).AppendAsync(
                $"post-{result}",
                expectedVersion: 0,
                Arg.Is<IReadOnlyCollection<object>>(events =>
                    events.OfType<PostCreatedEvent>().Any(e =>
                        e.PostId == result &&
                        e.AuthorId == _author.Id &&
                        e.Title == _post.Title &&
                        e.Description == _post.Description &&
                        e.Content == _post.Content
                    )
                )
            );
        }

        [Fact]
        public async Task ThenMustCallUnitOfWork()
        {
            _ = await _sut.Handle(_createPostCommand);

            await _unitOfWork.Received(1).CommitAsync();
        }
    }
}
