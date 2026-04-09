using TechnicalTest.Api.Models;
using TechnicalTest.Application.Commands;

namespace TechnicalTest.Api.Mappers
{
    public static class CreatePostRequestMapper
    {
        public static CreatePostCommand ToCommand(this CreatePostRequest request)
        {
            return new CreatePostCommand(
                request.AuthorId,
                request.Title,
                request.Description,
                request.Content,
                request.Author?.ToModel()
            );
        }
    }
}
