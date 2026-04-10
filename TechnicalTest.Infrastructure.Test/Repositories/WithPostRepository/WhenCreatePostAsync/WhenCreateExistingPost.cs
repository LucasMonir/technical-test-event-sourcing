using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.Infrastructure.Persistence.Services;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenCreatePostAsync
{
    public class WhenCreateExistingPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly Post _post;
        private readonly Author _author;
        private readonly PostRepository _sut;

        public WhenCreateExistingPost()
        {
            _author = AuthorBuilder.Default()
               .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _unitOfWork = new UnitOfWork(_dbContext);
            _sut = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustThrowDatabaseException()
        {
            await _sut.CreatePostAsync(_post);

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
            await _dbContext.Posts.AddAsync(_post);
            await _dbContext.SaveChangesAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
