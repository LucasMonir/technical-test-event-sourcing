using Microsoft.EntityFrameworkCore;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Domain;

namespace TechnicalTest.Infrastructure.Repositories
{
    internal class _sut(AppDbContext dbContext) : IAuthorRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Author?> GetPostAuthorAsync(Guid id)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> CreateAuthorAsync(Author author)
        {
            var entry = await _dbContext.Authors.AddAsync(author);

            return entry.Entity.Id;
        }
    }
}
