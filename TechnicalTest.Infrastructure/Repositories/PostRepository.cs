using Microsoft.EntityFrameworkCore;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Domain;

namespace TechnicalTest.Infrastructure.Repositories
{
    internal class PostRepository(AppDbContext dbContext) : IPostRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Post?> GetPostAsync(Guid id)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> CreatePostAsync(Post post)
        {
            var entry = await _dbContext.Posts.AddAsync(post);

            return entry.Entity.Id;
        }
    }
}