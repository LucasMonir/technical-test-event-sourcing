using FluentAssertions;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenGetPostAsync
{
    public class WithNoExistingPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _sut;
        private readonly Guid _id;

        public WithNoExistingPost()
        {
            _id = Guid.Empty;
            _dbContext = new TestDbContextFactory().Context;
            _sut = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustReturnNull()
        {
            var result = await _sut.GetPostAsync(_id);

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
