using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Services
{
    internal class PostQueryService(IPostRepository postRepository) : IPostQueryService
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<Post?> GetPostAsync(Guid id)
        {
            return await _postRepository.GetPostAsync(id);
        }

        public Task<Post> CreatePostAsync(Post post)
        {
            throw new NotImplementedException();

        }
    }
}
