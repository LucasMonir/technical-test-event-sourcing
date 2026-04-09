namespace TechnicalTest.Application.DTOs
{
    public record PostDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Content { get; set; }
    }
}
