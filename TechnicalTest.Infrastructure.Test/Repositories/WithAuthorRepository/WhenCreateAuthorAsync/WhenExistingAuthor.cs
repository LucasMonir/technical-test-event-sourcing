using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithAuthorRepository.WhenCreateAuthorAsync
{
    public class WhenExistingAuthor : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly AuthorRepository _authorRepository;
        private readonly Author _author;

        public WhenExistingAuthor()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _authorRepository = new AuthorRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustThrowDatabaseException()
        {
            await FluentActions.Invoking(async () =>
                await _authorRepository.CreateAuthorAsync(_author))
                .Should()
                .ThrowAsync<DbUpdateException>()
                .WithInnerException<DbUpdateException, SqliteException>();
        }

        #region Setup and Teardown
        async Task IAsyncLifetime.InitializeAsync()
        {
            await _dbContext.Authors.AddAsync(_author);
            await _dbContext.SaveChangesAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
