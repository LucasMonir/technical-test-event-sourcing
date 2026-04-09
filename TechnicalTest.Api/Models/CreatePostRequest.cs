namespace TechnicalTest.Api.Models
{
    public record CreatePostRequest
    {
        public Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Content { get; set; }
    }
}
