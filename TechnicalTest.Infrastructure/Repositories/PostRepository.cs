using Microsoft.EntityFrameworkCore;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.DTOs;

namespace TechnicalTest.Infrastructure.Repositories
{
    internal class PostRepository(AppDbContext dbContext) : IPostRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<PostDto?> GetPostAsync(Guid id,
            bool includeAuthor,
            CancellationToken cancellationToken = default)
        {
            var post = await _dbContext.Posts
                   .AsNoTracking()
                   .Where(p => p.Id == id)
                   .Select(p => new PostDto(
                       p.Id,
                       p.AuthorId,
                       p.Title,
                       p.Description,
                       p.Content,
                       null
                   ))
                   .FirstOrDefaultAsync(cancellationToken);

            if (post is null)
                return null;

            if (!includeAuthor)
                return post;

            var author = await _dbContext.Authors
                .AsNoTracking()
                .Where(a => a.Id == post.AuthorId)
                .Select(a => new AuthorDto(
                    a.Id,
                    a.Name,
                    a.Surname
                ))
                .FirstOrDefaultAsync(cancellationToken);

            return post with { Author = author };
        }
    }
}