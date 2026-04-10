using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Application.Abstractions.Persistence;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Repositories;
using TechnicalTest.Infrastructure.Services;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Repositories.WithAuthorRepository.WhenCreateAuthorAsync
{
    public class WhenExistingAuthor : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly _sut _sut;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Author _author;

        public WhenExistingAuthor()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _unitOfWork = new UnitOfWork(_dbContext);

            _sut = new _sut(_dbContext);
        }

        [Fact]
        public async Task ThenMustThrowDatabaseException()
        {
            await _sut.CreateAuthorAsync(_author);

            await FluentActions.Invoking(async () =>
                    await _unitOfWork.CommitAsync())
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
