using FluentAssertions;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Repositories;
using TechnicalTest.TestHelpers.Builders.Domain;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Repositories.WithPostRepository.WhenGetPostAsync
{
    public class WithExistingPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _postRepository;
        private readonly Post _post;
        private readonly Author _author;

        public WithExistingPost()
        {
            _dbContext = new TestDbContextFactory().Context;

            _author = AuthorBuilder.Default()
                .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _postRepository = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustReturnExpectedPost()
        {
            var result = await _postRepository.GetPostAsync(_post.Id);

            result.Should().Be(_post);
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
            _dbContext.Dispose();
        }
        #endregion
    }
}