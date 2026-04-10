using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.Infrastructure.Persistence.Services;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenCreatePostAsync
{
    public class WhenCreateNewPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _postRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly Post _post;
        private readonly Author _author;

        public WhenCreateNewPost()
        {
            _author = AuthorBuilder.Default()
               .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _unitOfWork = new UnitOfWork(_dbContext);
            _postRepository = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustCreateAndReturnPost()
        {
            await _postRepository.CreatePostAsync(_post);
            await _unitOfWork.CommitAsync();

            var saved = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == _post.Id);

            saved.Should().NotBeNull();
            saved.Should().BeEquivalentTo(_post);
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
