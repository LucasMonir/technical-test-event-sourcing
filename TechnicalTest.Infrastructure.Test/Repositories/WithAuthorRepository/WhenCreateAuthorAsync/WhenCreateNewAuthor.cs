using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithAuthorRepository.WhenCreateAuthorAsync
{
    public class WhenCreateNewAuthor : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly AuthorRepository _authorRepository;
        private readonly Author _author;

        public WhenCreateNewAuthor()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _authorRepository = new AuthorRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustCreateAndReturnAuthor()
        {
            var result = await _authorRepository.CreateAuthorAsync(_author);
            var saved = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Id == _author.Id);

            saved.Should().NotBeNull();
            saved.Should().BeEquivalentTo(_author);
        }

        #region Setup and Teardown
        public Task InitializeAsync() => Task.CompletedTask;

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
