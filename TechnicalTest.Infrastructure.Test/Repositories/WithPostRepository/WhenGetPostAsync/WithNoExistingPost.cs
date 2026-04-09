using FluentAssertions;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenGetPostAsync
{
    public class WithNoExistingPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _postRepository;
        private readonly Guid _id;

        public WithNoExistingPost()
        {
            _dbContext = new TestDbContextFactory().Context;
            _postRepository = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustReturnNull()
        {
            var result = await _postRepository.GetPostAsync(_id);

            result.Should().BeNull();
        }

        #region Setup and Teardown
        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
