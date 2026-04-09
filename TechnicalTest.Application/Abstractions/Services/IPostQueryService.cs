using TechnicalTest.Application.DTOs;

namespace TechnicalTest.Application.Abstractions.Services
{
    public interface IPostQueryService
    {
        Task<PostDto?> GetPostAsync(Guid id, bool includeAuthor);
    }
}
