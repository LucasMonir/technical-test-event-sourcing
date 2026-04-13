using FluentAssertions;
using TechnicalTest.Infrastructure.Repositories;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Repositories.WithPostRepository.WhenGetAsync
{
    public class WithNoExistingPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _sut;

        private readonly bool _includeAuthor;
        private readonly Guid _postId;

        public WithNoExistingPost()
        {
            _includeAuthor = true;
            _postId = Guid.NewGuid();

            _dbContext = new TestDbContextFactory().Context;
            _sut = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustReturnExpectedPostAndAuthorWhenIncludeAuthorTrue()
        {
            var result = await _sut.GetPostAsync(_postId, _includeAuthor);

            result.Should().Be(null);
        }

        #region Setup/Teardown
        public async Task InitializeAsync() { }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
