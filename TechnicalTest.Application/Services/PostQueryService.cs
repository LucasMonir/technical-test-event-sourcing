using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Mappers;

namespace TechnicalTest.Application.Services
{
    internal class PostQueryService(IPostRepository postRepository, IAuthorRepository authorRepository)
        : IPostQueryService
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IAuthorRepository _authorRepository = authorRepository;

        public async Task<PostDto?> GetPostAsync(Guid id, bool includeAuthor = false)
        {
            var post = await _postRepository.GetPostAsync(id);

            if (post is null)
                return null;

            if (!includeAuthor)
                return PostMapper.MapToDto(post);

            var author = await _authorRepository.GetPostAuthorAsync(post.AuthorId);

            return PostMapper.MapToDto(post, author);
        }
    }
}
