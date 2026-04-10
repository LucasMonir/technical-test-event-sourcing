using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Persistence;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain;
using TechnicalTest.Domain.Events;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Services.WithPostCommandHandler.WhenHandle
{
    public class WithNewPostAndExistingAuthor
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IPostRepository _postRepository;
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

            _authorRepository = Substitute.For<IAuthorRepository>();
            _authorRepository.GetPostAuthorAsync(_post.AuthorId)
                .Returns(_author);

            _postRepository = Substitute.For<IPostRepository>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventStore = Substitute.For<IEventStore>();

            _sut = new PostCommandHandler(_authorRepository,
                _postRepository,
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
