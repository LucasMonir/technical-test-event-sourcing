using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenCreatePostAsync
{
    public class WhenCreateNewPostWithNoExistingAuthor : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _postRepository;
        private readonly Post _post;

        public WhenCreateNewPostWithNoExistingAuthor()
        {
            _post = PostBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;

            _postRepository = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustThrowDatabaseException()
        {
            await FluentActions.Invoking(async () =>
                await _postRepository.CreatePostAsync(_post))
                .Should()
                .ThrowAsync<DbUpdateException>()
                .WithInnerException<DbUpdateException, SqliteException>();
        }

        #region Setup and Teardown
        async Task IAsyncLifetime.InitializeAsync()
        {
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
