using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Persistence;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Services.WithPostCommandHandler.WhenHandle
{
    public class WithNewPostAndNewAuthor
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IPostRepository _postRepository;
        private readonly IEventStore _eventStore;
        private readonly IUnitOfWork _unitOfWork;

        private readonly CreatePostCommand _createPostCommand;

        private readonly Author _author;
        private readonly Post _post;

        private readonly PostCommandHandler _sut;

        public WithNewPostAndNewAuthor()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _createPostCommand = new CreatePostCommand(
                null,
                _post.Title,
                _post.Description,
                _post.Content,
                new AuthorModel(
                    _author.Name,
                    _author.Surname
                )
            );

            _authorRepository = Substitute.For<IAuthorRepository>();
            _authorRepository.CreateAuthorAsync(Arg.Any<Author>())
                .Returns(_author.Id);

            _postRepository = Substitute.For<IPostRepository>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventStore = Substitute.For<IEventStore>();

            _sut = new PostCommandHandler(_authorRepository,
                _postRepository,
                _eventStore,
                _unitOfWork);
        }


        [Fact]
        public async Task ThenMustCallPostRepository()
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
        public async Task ThenMustCallAuthorRepository()
        {
            var result = await _sut.Handle(_createPostCommand);

            result.Should().NotBeEmpty();

            await _authorRepository.Received(1).CreateAuthorAsync(Arg.Is<Author>(p =>
                p.Name == _author.Name &&
                p.Surname == _author.Surname)
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
