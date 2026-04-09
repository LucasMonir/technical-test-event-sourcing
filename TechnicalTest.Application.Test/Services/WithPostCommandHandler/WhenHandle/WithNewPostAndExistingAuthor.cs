using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Services.WithPostCommandHandler.WhenHandle
{
    public class WithNewPostAndExistingAuthor
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IPostRepository _postRepository;

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
            _postRepository.CreatePostAsync(_post)
                .Returns(_post);

            _sut = new PostCommandHandler(_authorRepository, _postRepository);
        }


        [Fact]
        public async Task ThenMustCreateExpectedPost()
        {
            var result = await _sut.Handle(_createPostCommand);

            result.Should().BeEquivalentTo(_post, options => options
                .ComparingByMembers<Post>()
                .Excluding(x => x.Id)
            );
        }
    }
}
