namespace TechnicalTest.Application.Commands
{
    public record CreatePostCommand(Guid? AuthorId,
        string Title,
        string Description,
        string Content,
        AuthorModel? Author = null);
}
