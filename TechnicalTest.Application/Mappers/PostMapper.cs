using TechnicalTest.Application.DTOs;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Mappers
{
    public static class PostMapper
    {
        public static PostDto? MapToDto(Post? post, Author? author = null)
        {
            return post is null ? null
            : new PostDto(
                post.Id,
                post.AuthorId,
                post.Title,
                post.Description,
                post.Content,
                AuthorMapper.MapToDto(author));
        }
    }
}
