using TechnicalTest.Application.DTOs;

namespace TechnicalTest.Application.Abstractions.Repositories
{
    public interface IPostRepository
    {
        Task<PostDto?> GetPostAsync(Guid id, bool includeAuthor, CancellationToken cancellationToken = default);
    }
}
