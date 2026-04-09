namespace TechnicalTest.Application.DTOs
{
    public record PostDto(
        Guid Id,
        Guid AuthorId,
        string Title,
        string Description,
        string Content,
        AuthorDto? Author
    );
}
