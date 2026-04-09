using TechnicalTest.Application.DTOs;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Abstractions.Services
{
    public interface IPostQueryService
    {
        Task<PostDto?> GetPostAsync(Guid id);
        Task<Post> CreatePostAsync(Post post);
    }
}
