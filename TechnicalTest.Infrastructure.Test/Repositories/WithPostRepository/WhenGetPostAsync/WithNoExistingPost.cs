using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Infrastructure.Persistence.Repositories;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.WithPostRepository.WhenGetPostAsync
{
    public class WithNoExistingPost
    {
        private readonly AppDbContext _appDbContext;
        private readonly PostRepository _postRepository;
        private readonly SqliteConnection _connection;
        private readonly Guid _id;

        public WithNoExistingPost()
        {
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
        public async Task ThenMustReturnNull()
        {
            var result = await _postRepository.GetPostAsync(_id);

            result.Should().BeNull();

            DisposeAsync();
        }

        private async void DisposeAsync()
        {
            await _appDbContext.DisposeAsync();
            _connection.Close();
        }
    }
}
