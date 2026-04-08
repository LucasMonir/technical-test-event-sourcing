namespace TechnicalTest.Domain
{
    public class Post
    {
        public required Guid Id { get; set; }
        public required Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Content { get; set; }
    }
}
