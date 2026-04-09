using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain;
using TechnicalTest.TestHelpers.Builders;

namespace TechnicalTest.Application.Test.Services.WithPostQueryService.WhenGetPostAsync
{
    public class WithExistingPost
    {
        private readonly IPostRepository _postRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly PostQueryService _sut;
        private readonly Post _post;
        private readonly Author _author;

        public WithExistingPost()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _authorRepository = Substitute.For<IAuthorRepository>();
            _authorRepository.GetPostAuthorAsync(_post.AuthorId)
                .Returns(_author);

            _postRepository = Substitute.For<IPostRepository>();
            _postRepository.GetPostAsync(_post.Id)
                .Returns(_post);

            _sut = new PostQueryService(_postRepository,
                _authorRepository);
        }

        [Fact]
        public async Task ThenMustReturnExpectedPost()
        {
            var result = await _sut.GetPostAsync(_post.Id);

            result.Should().BeEquivalentTo(_post, options => options
                .ComparingByMembers<Post>()
                .ExcludingMissingMembers()
                );
        }

        [Fact]
        public async Task ThenMustReturnExpectedPostWithAuthorWhenIncluded()
        {
            var result = await _sut.GetPostAsync(_post.Id, includeAuthor: true);

            result.Should().NotBeNull();
            result.Author.Should().BeEquivalentTo(_author);
        }
    }
}
