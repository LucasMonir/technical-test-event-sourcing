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
        private readonly PostQueryService _sut;
        private readonly Post _post;

        public WithExistingPost()
        {
            _post = PostBuilder.Default()
                .Build();

            _postRepository = Substitute.For<IPostRepository>();
            _postRepository.GetPostAsync(_post.Id)
                .Returns(_post);

            _sut = new PostQueryService(_postRepository);
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
    }
}
