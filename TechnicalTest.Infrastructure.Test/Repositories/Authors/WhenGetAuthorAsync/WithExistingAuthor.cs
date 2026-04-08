using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Persistence.Repositories;

namespace TechnicalTest.Infrastructure.Persistence.Test.Repositories.Authors.WhenGetAuthorAsync
{
    public class WithExistingAuthor
    {
        [Fact]
        public async Task AddAndFetchPost_Works()
        {
            // Setup in-memory SQLite DB
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:") // in-memory DB
                .Options;

            // Open context
            await using var db = new AppDbContext(options);
            await db.Database.OpenConnectionAsync();
            await db.Database.EnsureCreatedAsync();

            // Seed author
            var author = new Author { Id = Guid.NewGuid(), Name = "Alice", Surname = "Clethus" };
            await db.Authors.AddAsync(author);
            await db.SaveChangesAsync();

            // Repository instance
            var repo = new PostRepository(db);

            // Add post
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = "Hello World",
                Content = "Some content",
                AuthorId = author.Id,
                Description = "Some text"
            };
            var added = await repo.CreatePostAsync(post);

            // Fetch post
            var fetched = await repo.GetPostAsync(post.Id);

            // Assertions
            Assert.NotNull(fetched);
            Assert.Equal("Hello World", fetched!.Title);
            Assert.Equal(author.Id, fetched.AuthorId);
        }
    }
}
