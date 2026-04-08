using TechnicalTest.Domain;

namespace TechnicalTest.Application.Abstractions.Services
{
    public interface IPostQueryService
    {
        Task<Post?> GetPostAsync(Guid id);
        Task<Post> CreatePostAsync(Post post);
    }
}
