using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.TestHelpers.Builders;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenCreatePostAsync
{
    public class WhenCreateNewPost : IAsyncLifetime
    {
        private readonly AppDbContext _appDbContext;
        private readonly PostRepository _postRepository;
        private readonly SqliteConnection _connection;
        private readonly Post _post;
        private readonly Author _author;

        public WhenCreateNewPost()
        {
            _author = AuthorBuilder.Default()
               .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _appDbContext = new AppDbContext(options);
            _appDbContext.Database.EnsureCreated();

            _postRepository = new PostRepository(_appDbContext);
        }

        [Fact]
        public async Task ThenMustCreateAndReturnPost()
        {
            var result = await _postRepository.CreatePostAsync(_post);
            var saved = await _appDbContext.Posts.FirstOrDefaultAsync(x => x.Id == _post.Id);

            saved.Should().NotBeNull();
            saved.Should().BeEquivalentTo(_post);
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            await _appDbContext.Authors.AddAsync(_author);
            await _appDbContext.SaveChangesAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _appDbContext.DisposeAsync();
            _connection.Close();
        }
    }
}
