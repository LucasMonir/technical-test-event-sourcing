using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain.Models;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Services.WithPostQueryService.WhenGetPostAsync
{
    public class WithExistingPost
    {
        private readonly IPostRepository _postRepository;
        private readonly PostQueryService _sut;
        private readonly Post _post;
        private readonly Author _author;
        private readonly bool _includeAuthor;

        public WithExistingPost()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _includeAuthor = true;

            PostDto postDto = new(_post.Id,
                _post.AuthorId,
                _post.Title,
                _post.Description,
                _post.Content,
                new AuthorDto(_author.Id,
                _author.Name,
                _author.Surname));

            _postRepository = Substitute.For<IPostRepository>();
            _postRepository.GetPostAsync(_post.Id, _includeAuthor)
                .Returns(postDto);

            _sut = new PostQueryService(_postRepository);
        }

        [Fact]
        public async Task ThenMustReturnExpectedPost()
        {
            var result = await _sut.GetPostAsync(_post.Id, _includeAuthor);

            result.Should().BeEquivalentTo(_post, options => options
                .ComparingByMembers<Post>()
                .ExcludingMissingMembers()
                );
        }

        [Fact]
        public async Task ThenMustReturnExpectedPostWithAuthorWhenIncluded()
        {
            var result = await _sut.GetPostAsync(_post.Id, _includeAuthor);

            result.Should().NotBeNull();

            result.Author.Should().BeEquivalentTo(_author, options => options
                .ComparingByMembers<Author>()
                .ExcludingMissingMembers()
                );
        }
    }
}
