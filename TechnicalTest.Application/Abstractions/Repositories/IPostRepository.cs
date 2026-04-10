using TechnicalTest.Domain;

namespace TechnicalTest.Application.Abstractions.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetPostAsync(Guid id);
        Task<Guid> CreatePostAsync(Post post);
    }
}
