namespace TechnicalTest.Api.Models
{
    public record CreatePostRequest(Guid? AuthorId,
        string Title,
        string Description,
        string Content,
        CreateAuthorRequest? Author = null);
}
