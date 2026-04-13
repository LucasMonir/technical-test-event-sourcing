using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.DTOs;

namespace TechnicalTest.Application.Services
{
    internal class PostQueryService(IPostRepository postRepository)
    : IPostQueryService
    {
        public readonly IPostRepository _postRepository = postRepository;

        public async Task<PostDto?> GetPostAsync(Guid id, bool includeAuthor = false)
        {
            var post = await _postRepository.GetPostAsync(id, includeAuthor);

            if (post is null)
                return null;

            return post;
        }
    }
}
