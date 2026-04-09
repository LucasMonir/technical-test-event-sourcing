using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Services;

namespace TechnicalTest.Application.Test.Services.WithPostQueryService.WhenGetPostAsync
{
    public class WithNonExistingPost
    {
        private readonly IPostRepository _postRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly PostQueryService _sut;

        public WithNonExistingPost()
        {
            _postRepository = Substitute.For<IPostRepository>();
            _authorRepository = Substitute.For<IAuthorRepository>();
            _postRepository.GetPostAsync(Arg.Any<Guid>())
                .Returns((Domain.Post?)null);

            _sut = new PostQueryService(_postRepository,
                _authorRepository);
        }

        [Fact]
        public async Task ThenMustReturnNull()
        {
            var result = await _sut.GetPostAsync(Guid.NewGuid());

            result.Should().BeNull();
        }
    }
}
