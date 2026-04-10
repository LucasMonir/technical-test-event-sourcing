using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Repositories;
using TechnicalTest.Infrastructure.Services;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Repositories.WithAuthorRepository.WhenCreateAuthorAsync
{
    public class WhenCreateNewAuthor : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly _sut _authorRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly Author _author;

        public WhenCreateNewAuthor()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _unitOfWork = new UnitOfWork(_dbContext);
            _authorRepository = new _sut(_dbContext);
        }

        [Fact]
        public async Task ThenMustCreateAndReturnAuthor()
        {
            await _authorRepository.CreateAuthorAsync(_author);
            await _unitOfWork.CommitAsync();

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
