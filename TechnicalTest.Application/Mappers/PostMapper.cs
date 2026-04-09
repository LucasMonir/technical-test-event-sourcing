using TechnicalTest.Application.DTOs;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Mappers
{
    public static class PostMapper
    {
        public static PostDto MapToDto(Post post)
        {
            return new()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Title = post.Title,
                Content = post.Content,
                Description = post.Description,
            };
        }
    }
}
