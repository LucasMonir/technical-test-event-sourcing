namespace TechnicalTest.Infrastructure.Models
{
    public class PostReadModel
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
