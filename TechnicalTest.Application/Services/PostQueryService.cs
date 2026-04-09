using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Mappers;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Services
{
    internal class PostQueryService(IPostRepository postRepository) : IPostQueryService
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<PostDto?> GetPostAsync(Guid id)
        {
            var post = await _postRepository.GetPostAsync(id);

            return post is null ? null : PostMapper.MapToDto(post);
        }

        public Task<Post> CreatePostAsync(Post post)
        {
            throw new NotImplementedException();

        }
    }
}
